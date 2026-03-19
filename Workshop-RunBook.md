Prerequisites: 
- Be sure SearXNG instance is open + has json format enabled
    sudo vim /var/www/searxng/settings.yml
    sudo yunohost service restart searxng
- Check n8n is working
- Check deployed app is working correctly + has open network settings
- Check foundry is deployed correctly
- Open all relevant pages (azure portal, n8n, searxng)

---

1. Slides about agents / agentic AI
2. Show .NET app
    a. show everything
    b. extend it to add headshot images via cloud agent
3. Create n8n agent
    a. from scratch, add connectors, add model
    b. test chat with simple "how are you?" prompt
    c. show monitor in foundry
    d. add SearXNG tool
    e. look up for famous italian people online 
    f. add an http tool to create new people on our app
        {{ $fromAI("fullName", "the name of the famous person", "string") }}
        {{ $fromAI("background", "the background of the famous person", "string") }}
        {{ $fromAI("competenceField", "the competence field of the famous person", "string")}}
4. Back to the .NET app
    a. show images with placeholder
    b. open step 2 workflow in n8n
    c. add headshot images by using n8n

Post-mortem:
- Close network for SearXNG
- Close network for deployed app
- Check if everything is fine

System Prompt:
You are an expert in collecting facts about famous people.
You always search the web when asked about famous people.
You have a collection of famous people that you can query and update if requested.
Remember you only answer about famous people and related topics, avoid all other subjects completely. Be polite but firm in your refusal when the user tries to ask something else.