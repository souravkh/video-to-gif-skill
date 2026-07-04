---
name: video-to-gif
description: Convert a video file to an animated GIF using MediaToolkit (FFmpeg wrapper).
---

# Video-to-gif

use video-to-gif skills to respond to user enter "video to gif" or "video2gif" or "v2gif" in chat

print  "video-2-gif" in ascii art.

Then get user folder location and list all video files in that folder in desc order by file size.

Then pass one one video to convert to gif run video-to-gif using scripts

The output folder will be {inputlocation}/Output/{videoname}.gif

After completion show gif names with location

then print "END!"  ascii art  in chat.


## Overview
This skill provides a simple command-line interface to turn any video (MP4, AVI, MOV, ...) into an animated GIF. It uses the **MediaToolkit** NuGet package (`v1.1.0.1`), which internally invokes **ffmpeg**.

The program accepts either a **single video file path** or a **directory path** (scans for `*.mp4` recursively). Output GIFs are saved in a sibling `Output/` folder next to the source file.

## Usage
```bash
# Single file
VideoToGifSkill.exe <path-to-video-file>

# Directory (converts all *.mp4 files found recursively)
VideoToGifSkill.exe <path-to-directory>
```

> **Note:** `ffmpeg` must be available on the system PATH. The tool verifies this at startup via `IsFfmpegAvailable()`.

## Output Path Convention
| Input | Output |
|---|---|
| `C:\Videos\clip.mp4` | `C:\Videos\Output\clip.gif` |
| `C:\Videos\` (directory) | `C:\Videos\<sub>\Output\<name>.gif` (per file) |

The `Output/` directory is created automatically if it does not exist.

## Files

| File | Purpose |
|---|---|
| VideoToGifSkill.csproj | .NET 8 (net8.0-windows) project — pulls `MediaToolkit` v1.1.0.1 and `System.Configuration.ConfigurationManager` v10.0.9 |
| Program.cs | Entry point — checks ffmpeg availability, reads input arg, dispatches to `Mp4Convertor` for file or directory mode |
| Convertor.cs | `Mp4Convertor.Convert(string input)` — resolves output paths and uses `MediaToolkit.Engine` to perform the conversion |
| OutputDirectoryHelper.cs | Creates/returns the `Output/` subdirectory beside the input file |
| OutputFileHelper.cs | Builds the output `.gif` file path from the input filename |

## How It Works (Program.cs flow)

```
1. IsFfmpegAvailable()         -> verifies ffmpeg is on PATH
2. args[0]                     -> input path (file or directory)
3. File.Exists(input)          -> single file  -> Mp4Convertor.Convert(input)
   Directory.Exists(input)     -> directory    -> enumerate *.mp4 recursively
                                               -> Mp4Convertor.Convert(mp4File) per file
4. OutputDirectoryHelper       -> ensures <inputDir>/Output/ exists
5. OutputFileHelper            -> resolves output as <outputDir>/<name>.gif
6. MediaToolkit.Engine         -> GetMetadata() + Convert() -> writes the GIF
```

## Prerequisites
- **.NET 8** runtime/SDK (net8.0-windows)
- **ffmpeg** installed and available on `PATH`
- NuGet packages (restored automatically on build):
  - `MediaToolkit` v1.1.0.1
  - `System.Configuration.ConfigurationManager` v10.0.9

## Build & Run
```bash
cd .agents/skills/video_to_gif/scripts
dotnet build
dotnet run -- <path-to-video-or-directory>
```

## Extending the Skill
- Swap **MediaToolkit** for another FFmpeg wrapper by editing `Convertor.cs`.
- To support more video formats (AVI, MOV, MKV...), update the `Directory.EnumerateFiles` pattern in `Program.cs` to include additional extensions.

---
