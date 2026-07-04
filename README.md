# video-to-gif-skill

An agent skill that converts video files (MP4) into animated GIFs. It's a small .NET 8 console tool wrapping **MediaToolkit** (an FFmpeg wrapper), packaged as a self-contained skill folder so it can be dropped into **Antigravity, Claude Code, GitHub Copilot, and OpenAI Codex**.

## Get started

- **[WHERE-TO-INSTALL.md](./WHERE-TO-INSTALL.md)** — copy `video-to-gif/` into the right folder for your agent (workspace or global).
- **[video-to-gif/SKILL.md](./agents/skills/video-to-gif/SKILL.md)** — what the skill does, how the agent should behave, and how to build/run it manually.

## What it does

- Accepts a **single video file** or a **directory** as input.
- If given a directory, it recursively scans for `*.mp4` files and processes them in descending order of file size.
- Converts each video to an animated `.gif` using `ffmpeg` via the `MediaToolkit` NuGet package.
- Writes output GIFs to a sibling `Output/` folder next to each source file.
- Prints the list of generated GIFs (with full paths) when finished.

## Repository layout

```
video-to-gif-skill/
├── README.md              # you are here
├── WHERE-TO-INSTALL.md     # placement table for each agent (workspace + global paths)
└── video-to-gif/           # the self-contained skill — copy this whole folder
    ├── SKILL.md
    └── scripts/
        ├── VideoToGifSkill.csproj
        ├── Program.cs
        ├── Convertor.cs
        ├── OutputDirectoryHelper.cs
        └── OutputFileHelper.cs
```

## Prerequisites

- **.NET 8 SDK** (project targets `net8.0-windows`)
- **ffmpeg** installed and available on your system `PATH`
- NuGet packages (restored automatically on build): `MediaToolkit` v1.1.0.1, `System.Configuration.ConfigurationManager` v10.0.9

## Manual build & run (no agent)

```bash
cd video-to-gif/scripts
dotnet build
dotnet run -- <path-to-video-or-directory>
```

## License

This project is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.
