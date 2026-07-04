# Where to install this skill

This folder — `video-to-gif/` — is a complete, self-contained skill:

```
video-to-gif/
├── SKILL.md       # Main instructions (required)
└── scripts/       # Helper scripts (the .NET tool that does the conversion)
```

To use it with an agent, copy or move the **entire `video-to-gif/` folder** (not just `SKILL.md`) into the correct location below for whichever agent(s) you use. Nothing inside the folder needs to change — the same folder works for every agent listed.

## Placement locations

| Agent | Location | Scope |
|---|---|---|
| **Antigravity** | `<workspace-root>/.agents/skills/video-to-gif/` | Workspace-specific |
| | `~/.gemini/config/skills/video-to-gif/` | Global (all workspaces) |
| **OpenAI Codex** | `<workspace-root>/.agents/skills/video-to-gif/` | Workspace-specific |
| | `~/.codex/skills/video-to-gif/` | Global (all workspaces) |
| **Claude Code** | `<workspace-root>/.claude/skills/video-to-gif/` | Workspace-specific |
| | `~/.claude/skills/video-to-gif/` | Global (all workspaces) |
| **GitHub Copilot** | `<workspace-root>/.github/skills/video-to-gif/` | Workspace-specific |
| | `~/.copilot/skills/video-to-gif/` | Global (all workspaces) |

`<workspace-root>` = the root folder of the project/repo you have open. `~` = your home directory (`/Users/you`, `/home/you`, or `C:\Users\you`).

Note: Antigravity and OpenAI Codex both read the same workspace path (`.agents/skills/`), so one copy there covers both.

## Example: installing for Claude Code (workspace-specific)

```bash
mkdir -p <workspace-root>/.claude/skills
cp -r video-to-gif <workspace-root>/.claude/skills/video-to-gif
```

## Example: installing for Codex (global, all workspaces)

```bash
mkdir -p ~/.codex/skills
cp -r video-to-gif ~/.codex/skills/video-to-gif
```

Repeat the same `cp -r video-to-gif <target-path>/video-to-gif` pattern for any other agent/location combination in the table above.

## After installing

Start (or restart) the agent, then trigger the skill by typing any of:

```
video to gif
video2gif
v2gif
```

The agent will ask for a folder, list the videos it finds (largest first), convert one to a GIF, and tell you where it saved it.
