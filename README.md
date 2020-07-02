# Hot Call-outs for GTA V

This is a branch from Hot Call-outs that upgrades Hot Call-outs from GTA IV LCPDFR to GTA V LSPDFR.

## Callouts

This project aimed to recreate all of the call-outs in original IV version. The IV version is updated - but only bug fixes and only small callouts will be added to IV version, and after this project completed recreation and successfully added 10 callouts based on completed callouts, the IV version will only receive bug fixes, and then, dropped.

### Currently Playable Callouts

- Dangerous Driver

  Thinks about the traffic of Los Santos. A driver is all over the road, and does not listen to traffic rules, running red lights, ramming other's cars, etc. You will need to arrest this guy.

- Car Thief

  A stolen vehicle has been found in the streets of San Andreas. Arrest the thief.

- Escaping Prisoner

  A prisoner has been found in street along with other person. Pull them over, arrest all of them.

### Callouts with Unresolvable Issued and Cannot Be Played

- Lack of Document

  - Uninsured Vehicle

    A vehicle has been found uninsured. Go and pull over, make him a ticket.

  - Unregistered Vehicle

    A vehicle has been found without Registration. Go and pull over, make him a ticket.

  *Reason of Cannot Be Played:* continuously reporting uncatchable `NullReferenceException` on `OnCalloutAccepted` method. 

  *Request Contribution*: need some assistance to locate and resolve the uncatchable `NullReferenceException` - go to `NullReference.md` to gather information I have found.

### Callouts currently WIP

* Visitors of Diamond Casino Resort causing trouble

  A visitor of Diamond Casino & Resort causing trouble and does not want to leave. Make him leave. Beware, they can run away.

  * Dialogue system is currently being redone.

## Known Issues

* Reporting suspect arrested by STP as dead
  * This is worked as intended by STP author and mostly wont fix
    * A workaround has been included. This will only work for STP takedowns.
* Suspect returning their vehicle or drives player's vehicle when callout ends
  * A workaround has been included to prevent suspects being dismissed if they are arrested. Beware, they does not disappear in the game world.
  * You cannot grab them when suspect insist to enter a vehicle.
  * Arrest them by STP or grab them by Arrest Manager and putting them in a car can prevent this issue.
    * If you use Arrest Manager to grab and make them enter your vehicle, do not transport them to Downtown (Mission Row) Police Station as it requires suspect to leave the car causing suspect to enter player's vehicle as driver. Simply transport them to other police stations.

## Build Instructions

You need .NET Framework 4.8 and Visual Studio 16.6.3 or later. Add following dependencies to References folder:

* RAGE Plugin Hook by MulleDK19 & LMS
* LSPD First Response by G17 Media
* Stop The Ped by BejoIjo
* Traffic Policer by Albo1125
* LSPDFR+ by Albo1125

And then use `devenv` command to build the solution, or simply use the IDE to build the solution. You can use **Debug** build option to access WIP callouts, but they may not work.