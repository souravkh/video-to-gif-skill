---
name: video-to-gif
description: Convert a video file (or a folder of videos) to an animated GIF using MediaToolkit (an FFmpeg wrapper). Trigger this skill when the user says "video to gif", "video2gif", or "v2gif" in chat.
---

# Video-to-GIF

## When to use this skill
Trigger on any of: `video to gif`, `video2gif`, `v2gif`.

## Agent behavior
Follow these steps in order:

1. Print `video-2-gif` as ASCII art in chat.
2. Ask the user for the input folder location (if not already provided).
3. List all video files in that folder, sorted in **descending order by file size**.
4. run the `video-to-gif` with user input location script (see [Build & Run](#build--run)) to convert it to a GIF.
5. The output is written to `{inputLocation}/Output/{videoName}.gif`.
6. After the conversion completes, show the generated GIF name(s) with their full location.
7. Print `END!` as ASCII art in chat.

## Overview
This skill provides a simple command-line tool to turn a video (MP4) into an animated GIF. It uses the **MediaToolkit** NuGet package (`v1.1.0.1`), which internally invokes **ffmpeg**.

The program accepts either a **single video file path** or a **directory path** (it scans for `*.mp4` files recursively). Output GIFs are saved in a sibling `Output/` folder next to the source file.

## Usage
```bash
# Single file
VideoToGifSkill.exe <path-to-video-file>

# Directory (converts all *.mp4 files found recursively, largest first)
VideoToGifSkill.exe <path-to-directory>
```

> **Note:** `ffmpeg` must be available on the system `PATH`. The tool verifies this at startup via `IsFfmpegAvailable()` and will not proceed if it's missing.

## Output path convention
| Input | Output |
|---|---|
| `C:\Videos\clip.mp4` | `C:\Videos\Output\clip.gif` |
| `C:\Videos\` (directory) | `C:\Videos\<sub>\Output\<name>.gif` (per file) |

The `Output/` directory is created automatically if it does not exist. An existing GIF with the same name is overwritten.

## Files in this skill
```
video-to-gif/
├── SKILL.md              # this file
└── scripts/
    ├── VideoToGifSkill.csproj
    ├── Program.cs             # entry point — checks ffmpeg, dispatches file/directory mode
    ├── Convertor.cs           # Mp4Convertor.Convert() — runs the actual conversion via MediaToolkit.Engine
    ├── ConverterFactory.cs   # factory for selecting appropriate converter
    ├── IVideoConverter.cs     # interface for video converters
    ├── OutputDirectoryHelper.cs   # creates/returns the Output/ subdirectory
    └── OutputFileHelper.cs        # builds the output .gif filename
```

## How it works (Program.cs flow)
```
1. IsFfmpegAvailable()         -> verifies ffmpeg is on PATH
2. args[0]                     -> input path (file or directory)
3. File.Exists(input)          -> single file  -> Mp4Convertor.Convert(input)
   Directory.Exists(input)     -> directory    -> enumerate *.mp4 recursively,
                                                  ordered by file size descending
                                               -> Mp4Convertor.Convert(mp4File) per file
4. OutputDirectoryHelper       -> ensures <inputDir>/Output/ exists
5. OutputFileHelper            -> resolves output as <outputDir>/<name>.gif
6. MediaToolkit.Engine         -> GetMetadata() + Convert() -> writes the GIF
```

## Prerequisites
- **.NET 8** runtime/SDK (`net8.0-windows`)
- **ffmpeg** installed and available on `PATH`
- NuGet packages (restored automatically on build):
  - `MediaToolkit` v1.1.0.1
  - `System.Configuration.ConfigurationManager` v10.0.9

## Build & run
From inside this skill's own folder (the one containing this `SKILL.md`):
```bash
cd scripts
dotnet build
dotnet run -- <path-to-video-or-directory>
```

## Extending the skill
- Swap **MediaToolkit** for another FFmpeg wrapper (e.g. `FFMpegCore`) by editing `scripts/Convertor.cs` — a commented-out `FFMpegCore` code path is already left there as a starting point.
- To support more video formats (AVI, MOV, MKV, ...), update the `Directory.EnumerateFiles` pattern in `scripts/Program.cs` to include additional extensions.
- The project currently targets `net8.0-windows`; retarget to `net8.0` if cross-platform (Linux/macOS) support is needed, and confirm `MediaToolkit`/`ffmpeg` behave as expected there.

---
