Create a task that runs
triggerTasks.vbs http://<yoururl>/schedule?key=uDG81TICusSd
This is CRITICAL for reports functioning!

***Building coffescript***
Weber (https://www.npmjs.com/package/weber) is used for coffeescript transpilation to JS. Unfortunatelly,
latest version is 5 years old as of 2017 and requires node 0.8.0 (could be higher, but 0.12.0 is too high.)
I used NVM for windows (https://github.com/coreybutler/nvm-windows), that allows easily switch between different node versions.
After installing it, you can install node:
	nvm install 0.8.0
	nvm use 0.8.0
Then restore packages: 
	npm install
Finally to run weber and transpile js and css:
	npm run build

