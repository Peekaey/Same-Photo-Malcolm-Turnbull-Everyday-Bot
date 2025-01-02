# Same Photo Malcolm Turnbull Everyday Bot

-------- 
Posts the same photo of Malcolm Turnbull everyday. That's it. 

Inspired from an old facebook page that did the same thing.

![malcolm turnbull](https://github.com/Peekaey/Same-Photo-Malcolm-Turnbull-Everyday-Bot/blob/master/src/Images/MalcolmTurnbull.jpg)

## About
Discord bot as a [.NET Generic Host](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host?tabs=appbuilder) using [Netcord](https://netcord.dev/) with docker support.  
Has some unneeded commands as reference/examples but can be easily removed when needed.

## Building & Running through Docker
1. Ensure you are in the root of the folder
2. ``docker build -t image_name .`` to build the image
3. ``docker run -d --name container_name -e DISCORD_BOT_TOKEN=you_token -e DISCORD_CHANNEL_ID=your_channel_id image_name`` to run the container
