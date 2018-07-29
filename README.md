# HueLightDJ
Hue Light DJ using Hue Entertainment API

[![Build status](https://ci.appveyor.com/api/projects/status/sdng57og0rpx76ub/branch/master?svg=true)](https://ci.appveyor.com/project/michielpost/huelightdj/branch/master)

## What is it?
This web app connects to a Philips Hue Bridge over the local network. It uses the Hue Entertainment API to update the lights almost instantly.
Hue Light DJ is meant for setups with 20+ Hue Lights. Don't use this app for your personal setup with less than 5 lights. Things might get interesting with 10 lights, you can try it out. There is also a DEMO mode build in so you see how it would look like on a 20+ light setup.

## Features
- Comes with a lot of build in effects
- BPM input to specify speed of effects
- Preview window, to see the result of the effects
- Random mode, runs a random effect on a random group
- Auto mode (Party Mode), starts a new random effect every 6 seconds
- Build in groups like front/back, left/right
- Random group, creates a new random group every time
- Effect Composer, try out new effects by selecting a group, IteratorMode and effect
- Brightness Slider to control overall brightness
- DEMO mode, to test the app without a Hue Bridge

## Screenshots
TODO

## Video
TODO

## Tech
- ASP.Net Core 2.1 backend
- SignalR for realtime communication between frontend and backend
- Q42.HueApi for communicating with the Hue Bridge
- Vue.js frontend
- PixiJS for WebGL preview window
- 

## Feature Wishlist
- More build in effects
- Support for a hardware controller using WebMidi
