# Headshot Feature Specification

This document captures the full design of the headshot feature so it can be re-implemented from scratch.

---

## Overview

Each `FamousPerson` carries an optional headshot URL (`HeadshotUrl`).  
- When a person is created without a URL, a default avatar is generated automatically from `ui-avatars.com`.  
- The URL is stored on the entity and flows through to the API response and the Razor Pages UI.  
- A dedicated endpoint allows callers to update only the headshot without touching any other fields.  
- The UI renders the image as a circular thumbnail on each person card; if the URL fails to load, a placeholder is shown via an `onerror` handler.

---

## Data model

### `Models/FamousPerson.cs`

Add one nullable property to the existing entity:

```csharp
/// <summary>
/// Optional URL to a headshot image for the person.
/// </summary>
public string? HeadshotUrl { get; set; }
```

---

## DTOs

### `DTOs/FamousPersonResponse.cs`

Add to the response:

```csharp
/// <summary>
/// Optional URL to a headshot image for the person.
/// </summary>
public string? HeadshotUrl { get; init; }
```

### `DTOs/CreateFamousPersonRequest.cs`

Add optional field (no `[Required]`, max 2000 chars):

```csharp
/// <summary>
/// Optional URL pointing to a headshot image for the person.
/// </summary>
[StringLength(2000)]
public string? HeadshotUrl { get; set; }
```

### `DTOs/UpdateFamousPersonRequest.cs`

Same optional field:

```csharp
/// <summary>
/// Optional URL pointing to a headshot image for the person.
/// </summary>
[StringLength(2000)]
public string? HeadshotUrl { get; set; }
```

### `DTOs/HeadshotRequest.cs` _(new file)_

Dedicated DTO for the headshot-only endpoint:

```csharp
using System.ComponentModel.DataAnnotations;

namespace AI_Workshop_App.DTOs;

/// <summary>
/// Request payload to set or update a person's headshot URL.
/// </summary>
public sealed class HeadshotRequest
{
    /// <summary>
    /// The URL pointing to the person's headshot image.
    /// </summary>
    [Required]
    [StringLength(2000)]
    public string HeadshotUrl { get; set; } = string.Empty;
}
```

---

## Service

### `Services/IFamousPersonService.cs`

Update `AddAsync` and `UpdateAsync` signatures to include an optional `headshotUrl` parameter, and add a new method:

```csharp
Task<FamousPerson> AddAsync(
    string fullName,
    string background,
    string competenceField,
    string? headshotUrl = null,
    CancellationToken cancellationToken = default);

Task<bool> UpdateAsync(
    Guid id,
    string fullName,
    string background,
    string competenceField,
    string? headshotUrl = null,
    CancellationToken cancellationToken = default);

/// <summary>
/// Updates only the headshot URL for the specified famous person.
/// </summary>
Task<bool> UpdateHeadshotAsync(
    Guid id,
    string? headshotUrl,
    CancellationToken cancellationToken = default);
```

### `Services/FamousPersonService.cs`

**`AddAsync`** — after creating the entity, set the headshot:

```csharp
// Assign a sensible default headshot URL when none provided.
person.HeadshotUrl = string.IsNullOrWhiteSpace(headshotUrl)
    ? $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(person.FullName)}&size=256"
    : headshotUrl.Trim();
```

**`UpdateAsync`** — only overwrite if the caller passed a non-null value:

```csharp
if (headshotUrl is not null)
{
    person.HeadshotUrl = string.IsNullOrWhiteSpace(headshotUrl) ? null : headshotUrl.Trim();
}
```

**`UpdateHeadshotAsync`** _(new method)_:

```csharp
public async Task<bool> UpdateHeadshotAsync(
    Guid id,
    string? headshotUrl,
    CancellationToken cancellationToken = default)
{
    var person = await dbContext.FamousPeople.FirstOrDefaultAsync(
        existingPerson => existingPerson.Id == id,
        cancellationToken);

    if (person is null)
    {
        return false;
    }

    person.HeadshotUrl = string.IsNullOrWhiteSpace(headshotUrl) ? null : headshotUrl?.Trim();
    await dbContext.SaveChangesAsync(cancellationToken);
    return true;
}
```

---

## Controller

### `Controllers/FamousPeopleController.cs`

**`CreateAsync`** — pass `request.HeadshotUrl` through to `AddAsync`.

**`UpdateAsync`** — pass `request.HeadshotUrl` through to `UpdateAsync`.

**`MapResponse`** — include `HeadshotUrl` in the response mapping:

```csharp
private static FamousPersonResponse MapResponse(FamousPerson person) =>
    new()
    {
        Id = person.Id,
        FullName = person.FullName,
        Background = person.Background,
        CompetenceField = person.CompetenceField,
        HeadshotUrl = person.HeadshotUrl
    };
```

**New endpoint** — headshot-only update:

```csharp
/// <summary>
/// Updates only the headshot URL for a famous person.
/// </summary>
[HttpPut("{id:guid}/headshot")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
public async Task<IActionResult> UpdateHeadshotAsync(
    Guid id,
    [FromBody] HeadshotRequest request,
    CancellationToken cancellationToken)
{
    if (request is null)
    {
        return BadRequest();
    }

    var updated = await famousPersonService.UpdateHeadshotAsync(id, request.HeadshotUrl, cancellationToken);
    return updated ? NoContent() : NotFound();
}
```

Route: `PUT /api/famouspeople/{id:guid}/headshot`

---

## Seed data

### `Data/AppDbInitializer.cs`

Add `HeadshotUrl` to each seeded person using colour-coded `ui-avatars.com` URLs:

| Person | HeadshotUrl |
|---|---|
| Ada Lovelace | `https://ui-avatars.com/api/?name=Ada+Lovelace&background=0D6EFD&color=fff&size=256` |
| Marie Curie | `https://ui-avatars.com/api/?name=Marie+Curie&background=7C3AED&color=fff&size=256` |
| Nelson Mandela | `https://ui-avatars.com/api/?name=Nelson+Mandela&background=059669&color=fff&size=256` |

---

## UI

### `Pages/Index.cshtml`

**CSS** — add a `.headshot` class inside the `<style>` block:

```css
.headshot {
    width: 64px;
    height: 64px;
    object-fit: cover;
    border-radius: 50%;
    flex-shrink: 0;
    background-color: #f3f4f6;
}
```

**Card markup** — wrap the name in a flex row alongside the image and add an `onerror` placeholder fallback:

```html
<div class="d-flex align-items-center gap-3">
    <img src="@(person.HeadshotUrl ?? "https://via.placeholder.com/64")"
         onerror="this.onerror=null;this.src='https://via.placeholder.com/64?text=No+Image';"
         alt="@person.FullName headshot"
         class="headshot" />
    <h2 class="h4 mb-0">@person.FullName</h2>
</div>
```

---

## HTTP examples (AI-Workshop-App.http)

Add `"headshotUrl"` to the create and full-update request bodies:

```json
{
  "fullName": "Grace Hopper",
  "background": "...",
  "competenceField": "Computer Science",
  "headshotUrl": "https://ui-avatars.com/api/?name=Grace+Hopper&size=256"
}
```

Add a headshot-only request:

```http
### Update only headshot for a person
PUT {{host}}/api/famouspeople/{{createdPersonId}}/headshot
Content-Type: {{contentType}}
Accept: {{contentType}}

{
  "headshotUrl": "https://example.com/images/grace.jpg"
}
```
