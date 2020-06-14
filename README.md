# HotCalloutsV

This is a branch from HotCallouts that upgrades HotCallouts from GTA IV LCPDFR to GTA V LSPDFR.

## Why Upgrade

The LCPDFR is unsupported. Fortunately, I received a legitimate copy of GTA V allows me to develop, play, test plugins and mods for GTA V.

Also, I hate unstable of GTA IV - One small move can crash the whole game, or LCPDFR - for example, `EquipWeapon` method can and will crash the game, and you can only play LCPDFR about 30 minutes before it crashes and you need to restart to play again.

## Callouts

This project aimed to recreate all of the call-outs in original IV version. The IV version is updated - but only bug fixes and only small callouts will be added to IV version, and after this project completed recreation and successfully added 10 callouts based on completed callouts, the IV version will only receive bug fixes, and then, dropped.

### Currently Playable Callouts

- Dangerous Driver

  Thinks about the traffic of Los Santos. A driver is all over the road, and does not listen to traffic rules, running red lights, ramming other's cars, etc. You will need to arrest this guy.

- Car Thief

  A stolen vehicle has been found in the streets of San Andreas. Arrest the thief.

- Visitors of Diamond Casino Resort causing trouble

  A visitor of Diamond Casino & Resort causing trouble and does not want to leave. Make him leave. Beware, they can run away.

- Escaping Prisoner

  A prisoner has been found in street along with other person. Pull them over, arrest all of them.

### Callouts with Unresolvable Issued and Cannot Be Played

- Lack of Document

  - Uninsured Vehicle

    A vehicle has been found uninsured. Go and pull over, make him a ticket.

  - Unregistered Vehicle

    A vehicle has been found without Registration. Go and pull over, make him a ticket.

  *Reason of Cannot Be Played:* continuously reporting uncatchable `NullReferenceException` on `OnCalloutAccepted` method. 

  *Request Contribution*: need some assistance to locate and resolve the uncatchable `NullReferenceException` - go to `NullReference.md` to gather informations I have found.