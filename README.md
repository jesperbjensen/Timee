Timee
=====
The little job service that could. 
 
**This is a alpha release. It should work, but alot of work is still missing. Use it at your own risk.**

![Screenshot](http://deldysoft.cloudapp.net/timee/screenshot.png)

About
-----
Timee is a little service that lets you run a set of URL's in a given interval. It's focus on being easy to setup, and easy to use.

Installation
------------
1. Download the [zip file containing the latest release](http://deldysoft.cloudapp.net/timee/1.0.0.0.zip). 
2. Put the content in a good place on your server. (Or on your local machine for testing)
3. Run INSTALL.bat as a **Administrator**.
4. Done! Open your browser (on the server) and goto http://localhost:8888, and configure the service.
5. (Optional) Add or change the port and hostname to listen to in the __app.config__ file. You can comma-seperate multiple hosts.

Features
--------
* Calls a URL using a given internal.
* UI for configuring jobs.
* Keeps a log of each job run, including a UI seeing the result.

FAQ
---

**What about security**:
Use your firewall, if you don't want it to be exposed to the outside.

**How to implement a job**:
Host a website, where a given URL triggers the job. If you return a success code, then Timee things it went okay. The return value will be saved by Timee in the logs.

**I don't want to use my website for long running jobs**:
Make another site, and implement them there. Nancy can be a great fit for this - you can even use a self hosted one.

**Can I call _any_ URL**:
Sure! Call whatever you like.

**What about running a service a fixed time a day?**:
Sounds like a great idea. Make a fork and implement that. :)

**What about sending e-emails is a job fails?**:
Sounds like a great idea. Make a fork and implement that. :)

Built on this stuff
--------------------
* [TopShelf](http://topshelf-project.com/) - for hosting the Windows Service.
* [Nancy](http://nancyfx.org/) - for hosting the dashboard.

Contribute
----------
I whould love for people to help me building this great little tool, and making it more robust. Contact me if you like to help, or create a work, and make a pull request.