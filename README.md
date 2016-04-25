# akumaki

Saves/restores windows's location after monitor turns off/on.

In some cases using DisplayPort to connect Monitors,
windows' location are changed unintentionally when monitor turns off/on.

As a workaround for the problem, 
this application saves windows's location when monitor turns off,
and restores windows's location when monitor turns on.

## Installation

1. Download a binary from the release page:  https://github.com/knhr/akumaki/releases
2. Unzip the file

## Usage

#### How to start the application

Just execute akumaki.exe.

The application stays in your TaskTray.

We recommend that you create a shortcut of akumaki.exe to your StartUp folder.

#### How to quit the application

Right-click the application icon in TaskTray, and click the Exit menu.


## Advanced Settings

Windows' location cannot be moved immediately after monitor turns on.
So, this application restores windows' location a few second after monitor turns on.

How long needs to wait for depends on hard-wares.
Default wait-time is 4000msec.
If restoring windows' location fails, try changing the wait-time as follows.

1. Open akumaki.exe.config file.
2. Change the value of 'WaitTimeAfterMonitorOn'. <br />
(Unit of the value is millisecond. Value between 3500msec -- 5000msec is recommended.)

## History

#### V1.2 / 2016-04-25
* Allow user to specify wait-time after monitor turns on in the *.exe.config.

#### V1.1 / 2016-04-25
* Minor bug fix

#### v1.0 / 2016-04-23
* Pre-release
