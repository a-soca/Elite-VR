# Movement Analysis
## Basics
- Ships (all objects including stations, planets and stars) are located relative to the player (the player is the origin 0,0,0)
- When the player moves, in the game they remain stationary and the universe and all objects move opposite to the direction of travel
- When the player rotates, the ships will revolve around the origin

## Local Bubble
- The planet and star cannot exist at the same time as they use the same memory slot
- When the player moves within safe range of the space station, the sun is swapped out for the planet
- The planet/sun occupy the first ship memory block at `k% (&8200)`

## Reading Coordinates
- According to documentation, in memory the coordinates are arranged in 3 byte blocks for the first 8 bytes of a ship memory block

| Byte number | Format               |
| ----------- | -------------------- |
| 0-2         | `x_sign, x_hi, x_lo` |
| 3-5         | `y_sign, y_hi, y_lo` | 
| 6-8         | `z_sign, z_hi, z_lo` | 

- However it should be noted that each byte block is in little endian, so the true format for reading each byte is `?_lo, ?_hi, ?_sign`
- This leaves us with the final format for xyz coordinates: `x_lo, x_hi, x_sign, y_lo, y_hi, y_sign, z_lo, z_hi, z_sign`
- The `?_sign` values show that the coordinate is positive if `00` and negative if `80`. This is because bit 7 is used as the flag for positive/negative and `0b10000000` = `0x80`
- An example translation would be `8C 24 80 95 1A 00 C5 98 00` = `x:-9360` `y:+6805` `z:+39109`
- Achieved by (using x as an example) noting byte 3 is `80` so negative, concatenating `24` and `8C` to a single hex `0x248C` and translating to decimal `9360` resulting in the final answer `-9360`

## Accessing Coordinates
- These values can be watched via the debugger command `watch p 8200 b` repeated for all 8 byte locations (make sure to attach the debugger to the parasite)
- After this, using the decimal checkbox you may switch the hexadecimal code to an integer

## Proof of Concept
- By not touching the controls of the ship, it will spawn in flying directly towards the planet at a constant speed
- If the theory is correct, the z coordinate should decrease linearly if sampled at regular intervals as the planet moves nearer to the origin (the player)

### Data Collected
```
xl xh +- yl yh +- zl zh +- | x   y   z
00 00 00 00 00 00 A8 DC 00 | 0   0   56488
00 00 00 00 00 00 E8 DB 00 | 0   0   56296
00 00 00 00 00 00 34 DB 00 | 0   0   56116
00 00 00 00 00 00 98 DA 00 | 0   0   55960
00 00 00 00 00 00 F0 D9 00 | 0   0   55792
00 00 00 00 00 00 54 D9 00 | 0   0   55636
00 00 00 00 00 00 AC D8 00 | 0   0   55468
```

### Data Graphed
![Graphed Data of Z Coordinate Over Time](/Assets/Images/Movement-Analysis/ZCoordinateOverTime.PNG)

### Analysis
- The graph shows that this method of coordinate calculation is accurate for determining the position of ships in the local bubble
- Auxilary functions for translating the memory to simple coordinates to be read by the client will be required
- It will also be necessary to watch the entire block of memory, not just the coordinates of each ship, therefore some interpolation may be required on the client end to reduce the load on the server and transport layer through periodic sampling

## Using the Data to Map The Local Bubble (Sanity Check)
- By taking a snapshot of the entire block of memory, we can calculate the coordinates of each ship in `k%` and generate a map of the Local Bubble to demonstrate the method works beyond just the planet
- The debugger command `m p 8200 432` can be used to dump the entire content of the `k%` memory space
- The start of each ship block can be calculated by adding 37 (decimal) or `0x25` to the start of the previous block (starting from 8200)
- This is because the size of each block is 36 bytes
- There are 12 possible slots for ships/objects to be allocated and the slots are always populated at the lowest slot possible and when a ship is removed the memory is shuffled back to fill the gap, i.e. the first empty slot is the end of all ships in the local bubble

| Slot | Address |
| ---- | ------- |
| Planet/Sun | 8200 |
| Station | 8225 |
| Ship 1  | 824A |
| Ship 2  | 826F |
| Ship 3  | 8294 |
| Ship 4  | 82B9 |
| Ship 5  | 82DE |
| Ship 6  | 8303 |
| Ship 7  | 8328 |
| Ship 8  | 834D |
| Ship 9  | 8372 |
| Ship 10 | 8397 |

- Note: The `k%` memory space ends at 83B0

### Output of Memory Dump
```
       0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F 0123456789ABCDEF
8200  00 00 00 00 00 00 F4 F0 00 C4 D6 7F A8 10 01 BD ...............
8210  A6 49 57 1D 8B 96 82 90 07 18 5E 00 00 7F 7F 10 .IW.......^...
8220  00 00 00 00 0C 00 00 00 01 00 80 00 10 80 00 00 ................
8230  00 00 00 60 F0 5F 01 03 00 80 01 04 00 E0 00 00 ...`._..........
8240  00 00 FF 00 16 81 00 0D F0 00 66 6E 6C 60 B7 60 ..........fnl`.`
8250  7A 00 60 BA 73 BA B2 66 03 E8 B2 66 00 70 6B 6A z.`.s..f...f.pkj
8260  73 00 73 DD 67 76 60 77 00 03 B6 70 B3 00 6B 
...
83B0
```

- From this sector, it is evident that only the first 2 ships are present (the planet and the station) as the third block's xyz coordinates do not follow the correct pattern of `00`/`80` in byte 3,6,9 
- By waiting some time and flying around, ships will spawn

```
       0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F 0123456789ABCDEF
8200  82 0B 00 B2 0A 80 16 D8 00 60 0C EE 1E 52 D9 BF .........`...R..
8210  AD E9 CC 86 A0 5E 52 5D AB 03 83 00 00 7F 7F 10 .....^R]......
8220  00 00 00 00 0C 5B 07 80 73 0A 00 8E 27 80 00 00 .....[..s...'...
8230  00 82 00 60 3F 5C 5A 14 08 00 12 17 76 DB 1C 80 ...`?\Z.....v...
8240  00 00 FF 00 16 81 00 0D F0 00 EB 06 80 5F 05 00 ............._..
8250  88 11 00 00 00 00 02 00 60 00 00 00 E0 00 02 00 ........`.......
8260  E0 00 00 F1 00 0A 00 80 80 18 F1 6A CF 20 61 76 ...........j. av
8270  6E B8 03 60 6C 6F BC 6A A3 00 6B 7A 73 B3 70 73 n..`lo.j..kzs.ps
8280  62 A6 03 00 70 6B BA 77 03 E9 82 00 AE E8 B8 A6 b...pk.w........
8290  00 73 6C 73 
...
83B0
```

- Here, a ship has spawned and filled block 3

### Extracting Relevant Data
```
Planet  82 0B 00 B2 0A 80 16 D8 00
Station 5B 07 80 73 0A 00 8E 27 80
Ship 1  EB 06 80 5F 05 00 88 11 00
```

### Calculating the Coordinates
| Slot     | x     | y     | z      |
| -------- | ----- | ----- | ------ |
| Planet   | 2946  | -2738 | 55318  |
| Station  | -1883 | 2675  | -10126 |
| Ship 1   | -1771 | 1375  | 4488   |

### Graphing the Snapshot
- MATLAB was used to quickly plot the coordinates of the objects
```
cla; % Clears axis
clf; % Clears Figure

hold on % Do not overwrite plots
grid on % Add the grid to the axes

ax = gca; % Get the plot axes and set as variable
ax.GridColor = [1, 1, 1]; % Set the gridline colour to grey
ax.Color = [0,0,0]; % Set the background colour to black

plot3(0, 0, 0, '.', 'LineWidth', 10, 'MarkerSize', 10); % Plots the player as a dot
plot3(2946, -2738, 55318, 'o', 'LineWidth', 10, 'MarkerSize', 10) % Plots the planet as a circle
plot3(-1883, 2675, -10126, '+', 'LineWidth', 10, 'MarkerSize', 10); % Plots the Station as a plus
plot3(-1771, 1375, 4488, 'x', 'LineWidth', 10, 'MarkerSize', 10); % Plots the first ship as a cross

axis vis3d % Prevent autoscale
rotate3d on % Turn on 3d rotation

% Add a border to ensure entire figure is captured by exportgraphics
a = annotation('rectangle', [0 0 1 1], 'Color', 'k');

for az = 0 : 5 : 360 % Cycle azimuth through 360 degrees at 0.02 deg per second
    view(az, 30) % Update the view
    drawnow % Render
    exportgraphics(gca, 'graph.gif', 'Append', true); % Export the frame as a figure for the gif
end 

delete(a) % Remove the annotation
```

#### Resulting Plot
![Local Bubble Capture](/Assets/Images/Movement-Analysis/LocalBubblePlot.gif)