# DiscordBanDetector

This project took me less than 30 minutes to code, the way this works is pretty simple.

You fill in a token, and it will check every 24 hours if you are banned from discord. If it is the case, it'll let you know and will give you a list of friends and servers you had,
and if it isn't, it'll just save it all.

Just like my other Nitro Sniper project, this doesn't saves the tokens as plaintext, but instead, encrypts them once again with AES, and the key being your machine's guid, and the salt is hard-coded.
You can obviously modify / improve that.

One thing that would take 5 minutes to do is change the project type from console to normal application, so the console doesnt shows up for a millisecond everytime u launch it.
This might be annoying when in game or such.
