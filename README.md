# HueLightDJ
Hue Light DJ using Hue Entertainment API

[![Build .NET](https://github.com/michielpost/HueLightDJ/actions/workflows/build.yml/badge.svg)](https://github.com/michielpost/HueLightDJ/actions/workflows/build.yml)

# LIVE DEMO
NOTE: You can't use this demo to connect to your own bridge. But you can use the DEMO GROUP to view a demo/simulation.  

Click Start under Demo Group, then click Connect  
DEMO:  [https://huelightdj.azurewebsites.net/](https://huelightdj.azurewebsites.net/)

## What is it?
This web app connects to a Philips Hue Bridge over the local network. It uses the Hue Entertainment API to update the lights almost instantly.
Hue Light DJ is meant for setups with 20+ Hue Lights. You can also use this for your personal setup with less than 5 lights, but things get interesting with 10+ lights, you can try it out. There is also a DEMO mode build in so you see how it would look like on a 20+ light setup.

NOTE: [Hue Entertainment](https://developers.meethue.com/entertainment-blog) supports max 20 lights in an Entertainment Group using the v2 API. To get this to work with more than 20 addressable lights, you need to have 1 bridge for every 20 lights. A led strip contains more than 1 addressable light (for example 3 or 5).

### Demo with 32 Hue Light Strips on 5 bridges
[![Hue Entertainment demo with 32 Hue Light Strips](screenshots/vimeo_preview2.png)](https://vimeo.com/292273983) [![Hue Light DJ with 32 Hue LED strips](screenshots/vimeo_preview.png)](https://vimeo.com/290011309)

## Features
- Comes with a lot of build in effects
- BPM input to specify speed of effects
- Preview window, to see the result of the effects
- Random mode, runs a random effect on a random group
- Auto mode (Party Mode), starts a new random effect every 6 seconds
- Build in groups like front/back, left/right
- Random group, creates a new random group every time
- Touch effect, click or touch the preview area to start an effect from that position
- Brightness Slider to control overall brightness
- DEMO mode, to test the app without a Hue Bridge
- Connect to multiple Hue Bridges at the same time to control more than 20 lights 
- Bridge API key is stored in the browser's local storage

## Tech
- ASP.Net Core 9.0 backend
- SignalR for realtime communication from server to client
- gRPC for client to server communication
- [HueApi](https://github.com/michielpost/Q42.HueApi) for communicating with the Hue Bridge
- Blazor frontend
- PixiJS for WebGL preview window

## **Build and Install Instructions**
- Make sure to have [.Net 9.0](https://dotnet.microsoft.com/download) installed to build this project (`dotnet build`)
- Run the HueLightDJ.Web project(`cd HueLightDJ.BlazorWeb\Server && dotnet run`) 
- Follow the instructions to link your bridge or use the DEMO setup

## Docker
https://hub.docker.com/r/michielpost/huelightdj/
```
docker pull michielpost/huelightdj
docker run -d -p 8080:80 michielpost/huelightdj
```
HueLightDJ is now available on port 8080

## Feature Wishlist
- Multi Bridge light location configuration (use the Hue App to configure your light positions)
- Keyboard shortcuts
- Effect Composer, try out new effects by selecting a group, IteratorMode and effect
- More build in effects
- Support for a hardware controller using WebMidi

## Screenshots
![Main](screenshots/v3_01_main.png)
![Setup](screenshots/v3_02_setup.png)
![Multi Bridge Location Editor (v2 version)](screenshots/screenshot03_location_editor.png)

#### Disclaimer
This app is not affiliated with the Light DJ series of light show apps by NRTHRNLIGHTS, LLC
