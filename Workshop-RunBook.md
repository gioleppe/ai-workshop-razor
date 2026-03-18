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
2. Create n8n agent
    a. from scratch, add connectors, add model
    b. test chat with simple "how are you?" prompt
    c. show monitor in foundry
    d. add SearXNG tool
    e. look up for famous italian people online 
    f. add an http tool to create new people on our app
        {{ $fromAI("fullName", "the name of the famous person", "string") }}
        {{ $fromAI("fullName", "the background of the famous person", "string") }}
        {{ $fromAI("competenceField", "the competence field of the famous person", "string")}}


Post-mortem:
- Close network for SearXNG
- Close network for deployed app
- Check if everything is fine