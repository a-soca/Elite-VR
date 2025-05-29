# Source Code Analysis

| Index                                             |
| ------------------------------------------------- |
| [Info](#info)                                     |
| [Debug Commands](#beebem-debug-instructions)      |
| [Subroutines](#subroutines)                       |
| [Workspaces](#workspaces)                         |
| [Parasite Memory Map](#parasite-memory-map)       |
| [IO Memory Map](#io-memory-map)                   |
| [Ship Data Block Layout](#ship-data-block-layout) |

## Info
- [Elite (Second Processor Edition) Deep Dives](https://elite.bbcelite.com/deep_dives)
- PLEASE USE THE ELITE COMPENDIUM VERSION (Elite 6502 Second Processor Option) [Link](https://elite.bbcelite.com/versions/elite_compendium/elite-compendium-bbc-micro.dsd)


## BeebEm Debug Instructions
- `m p [Location] [Size]` will dump memory from the specified location on the **Parasite** (E.g. to dump K%, execute `m p 8200 432`)
- `m [Location] [Size]` will dump memory from the specified location on the main processor

## Subroutines

## Workspaces
### INWK
- Internal Workspace
- The zero-page internal workspace for the **Current Ship**
- the ship data is first copied from the ship data blocks at K% into INWK (or, when new ships are spawned, from the blueprints at XX21)

### K%
- [Ship Data Blocks](#Ship-Data-Block-Layout)
- Contains ship data for all the ships, planets, suns and space stations in our local bubble of universe
- The blocks are pointed to by the lookup table at location UNIV
- The first 720 bytes of the K% workspace hold ship data on up to 20 ships, with 37 (NI%) bytes per ship.
- Memory location: `&8200 - &84E4`

### UNIV
- Table of pointers to local universe's ship data blocks
- Referenced whenever a ship data block is needed to grab the raw data from K%

## Parasite Memory Map

```
  +-----------------------------------+   &FFFF
  |                                   |
  | Second Processor OS               |
  |                                   |
  +-----------------------------------+   &F800
  |                                   |
  | &F102-&F7FF unused                |
  |                                   |
  +-----------------------------------+   &F102
  |                                   |
  | Ship blueprints                   |
  |                                   |
  +-----------------------------------+   &D000 = XX21
  |                                   |
  | Ship line heap descends from LS%  |
  |                                   |
  +-----------------------------------+   SLSP
  |                                   |
  .                                   .
  .                                   .
  .                                   .
  .                                   .
  .                                   .
  |                                   |
  +-----------------------------------+   &9200
  |                                   |
  | LP workspace (shared with ships)  |
  |                                   |
  +-----------------------------------+   &8600 = LP
  |                                   |
  .                                   .
  .                                   .
  .                                   .
  .                                   .
  .                                   .
  |                                   |
  +-----------------------------------+   &84E4 when all ship slots are used
  |                                   |
  | Ship data blocks ascend from K%   |
  |                                   |
  +-----------------------------------+   &8200 = K%
  |                                   |
  | &818F-&81FF unused                |
  |                                   |
  +-----------------------------------+   &818F = F%
  |                                   |
  | Main parasite code (P.CODE)       |
  |                                   |
  +-----------------------------------+   &1000 = Parasite variables
  |                                   |
  | &0E3C-&0FFF unused                |
  |                                   |
  +-----------------------------------+   &0E3C
  |                                   |
  | WP workspace                      |
  |                                   |
  +-----------------------------------+   &0D00 = WP
  |                                   |
  | Hangar ship line heap, file space |
  |                                   |
  +-----------------------------------+   &0B00
  |                                   |
  | &0975-&0AFF unused                |
  |                                   |
  +-----------------------------------+   &0975
  |                                   |
  | UP workspace                      |
  |                                   |
  +-----------------------------------+   &0800 = UP
  |                                   |
  | Sine, cosine and arctan tables    |
  |                                   |
  +-----------------------------------+   &07C0 = SNE
  |                                   |
  | Recursive text tokens (WORDS.bin) |
  |                                   |
  +-----------------------------------+   &0400 = QQ18
  |                                   |
  | Second Processor OS workspace     |
  |                                   |
  +-----------------------------------+   &0200
  |                                   |
  | 6502 stack descends from &01FF    |
  |                                   |
  +-----------------------------------+
  |                                   |
  .                                   .
  .                                   .
  .                                   .
  .                                   .
  .                                   .
  |                                   |
  +-----------------------------------+
  |                                   |
  | Heap space ascends from XX3       |
  |                                   |
  +-----------------------------------+   &0100 = XX3
  |                                   |
  | MOS workspace                     |
  |                                   |
  +-----------------------------------+   &00EE
  |                                   |
  | Zero page workspace               |
  |                                   |
  +-----------------------------------+   &0000 = ZP
  ```

  ## IO Memory Map

```
  +-----------------------------------+   &FFFF
  |                                   |
  | Machine Operating System (MOS)    |
  |                                   |
  +-----------------------------------+   &C000
  |                                   |
  | Paged ROMs                        |
  |                                   |
  +-----------------------------------+   &8000
  |                                   |
  | &7E00-&7FFF unused                |
  |                                   |
  +-----------------------------------+   &7E00
  |                                   |
  | Memory for the split-screen mode  |
  |                                   |
  +-----------------------------------+   &4000
  |                                   |
  | &3D36-&3FFF unused                |
  |                                   |
  +-----------------------------------+   &3D36
  |                                   |
  | Main I/O code (I.CODE)            |
  |                                   |
  +-----------------------------------+   &2300 = TABLE
  |                                   |
  | &1900-&22FF unused                |
  |                                   |
  +-----------------------------------+   &1900
  |                                   |
  | MOS workspace                     |
  |                                   |
  +-----------------------------------+   &0800
  |                                   |
  | Tube host code                    |
  |                                   |
  +-----------------------------------+   &0400
  |                                   |
  | MOS workspace                     |
  |                                   |
  +-----------------------------------+   &0200
  |                                   |
  | 6502 stack descends from &01FF    |
  |                                   |
  +-----------------------------------+   &0100
  |                                   |
  | Zero page workspace               |
  |                                   |
  +-----------------------------------+   &0080 = ZP
  |                                   |
  | Tube host code                    |
  |                                   |
  +-----------------------------------+   &0000
```

## Ship Data Block Layout

| Location      | Description                                                                                                                                                                   |
| ------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Bytes \#0-2   | Ship's x-coordinate in space = `(x_sign x_hi x_lo)`                                                                                                                           |
| Bytes \#3-5   | Ship's y-coordinate in space = `(y_sign y_hi y_lo)`                                                                                                                           |
| Bytes \#6-8   | Ship's z-coordinate in space = `(z_sign z_hi z_lo)`                                                                                                                           |
| Bytes \#9-14  | Orientation vector nosev = `[ nosev_x nosev_y nosev_z ]`                                                                                                                      |
| Bytes \#15-19 | Orientation vector roofv = `[ roofv_x roofv_y roofv_z ]`                                                                                                                      |
| Bytes \#21-26 | Orientation vector sidev = `[ sidev_x sidev_y sidev_z ]`                                                                                                                      |
| Byte \#27     | Speed                                                                                                                                                                         |
| Byte \#28     | Acceleration                                                                                                                                                                  |
| Byte \#29     | Roll counter                                                                                                                                                                  |
| Byte \#30     | Pitch counter                                                                                                                                                                 |
| Byte \#31     | <ul> <li> Exploding state </li> <li> Killed state </li> <li> "Is being drawn on-screen" flag </li> <li> "Is visible on the scanner" flag </li> <li> Missile count </li> </ul> |
| Byte \#32     | <ul> <li> AI flag </li> <li> Hostility flag </li> <li> Aggression level </li> <li> E.C.M. flag </li> </ul>                                                                    |
| Bytes \#33-34 | Ship line heap address pointer                                                                                                                                                |
| Byte \#35     | Energy level                                                                                                                                                                  |
| Byte \#36     | NEWB flags (enhanced versions only)                                                                                                                                           |

- From this, we can deduce that bytes 0-8 in each 37 byte block stored in [K%](#k), starting from the address referenced by the lookup table in [UNIV](#univ) are the x, y and z ship coordinates
- Note: BBC Micro Second Processor Edition is considered an Enhanced Version

- For analysis on the coordinate system, please see the [Movement Analysis](/Documentation/Research/Movement-Analysis.md) page