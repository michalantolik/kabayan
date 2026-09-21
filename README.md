# Kabayan

## Why this project exists

The human goal is simple: make living and working in Poland easier for Filipinos.

**Kabayan** is the project/repository identity. **Kabayan Poland** is the chosen public name for the initial Poland market. The intended primary public domain is **`kabayanpoland.com`**, subject to live availability confirmation and registration. `Pinoy Poland` was the earlier working name and is retained only in decision history where useful.

No first problem or product capability has been validated. Work, documents, housing, healthcare, language, community, and other topics are areas to investigate, not a feature list.

## Current direction

The project starts by listening rather than guessing. The survey foundation exists, and a responsive **Discovery Landing V1** is implemented as a dependency-free static prototype. It explains the project, invites participation in the survey, and makes the discovery process visible. It is a discovery tool, not the first validated product capability.

```text
LISTEN -> LEARN -> CHOOSE -> BUILD -> LEARN AGAIN -> repeat
  ^
CURRENT PUBLIC STAGE
```

The prototype must reflect only what exists now and must not advertise hypothetical product areas as available services. Naming alternatives and voting are no longer part of the prototype: the public name is **Kabayan Poland**. Pilot feedback should instead test what that name makes people expect and whether it creates confusing or unwanted associations.

The next evidence-producing step remains **Pilot 0: trusted first review** with Gladys. The origin story and Filipino copy remain review material, and personal elements involving Gladys still require explicit publication approval. Her review is a first reality check, not representative evidence about Filipinos generally.

## Market scope

Poland is the only current market. A repeatable solution may later be explored independently for Filipinos in Germany, Austria, Switzerland, or elsewhere, but that is a future evidence-gated option rather than current scope.

Implementation should therefore solve Poland simply while avoiding unnecessary Poland-only coupling. Do not build multi-market abstractions before a real second market creates a concrete need.

## Run the local prototype

Open `index.html` directly in a browser. If a local server is preferred, run `python -m http.server 8000` from the repository root and open `http://localhost:8000`.

The prototype has no build step, external dependencies, analytics, backend, or persistent storage. Its survey action explains the planned participation journey but does not submit a survey.

## Public review deployment

GitHub Pages is configured through GitHub Actions and publishes only `index.html`, `styles.css`, and `script.js`. The workflow's existence does not prove that a deployment has run. Treat any Pages deployment as public.

Before broader sharing, confirm the publication boundary in [Design direction](docs/design.md), secure/configure the intended domain if available, and publish only personal elements explicitly approved by Gladys.

## Repository map

- [Vision](docs/vision.md) — human goal, Poland-first scope, and evidence-gated future markets
- [Brand and market strategy](docs/brand-and-market-strategy.md) — chosen identity, domain safeguards, future-market boundary, and implementation safety check
- [Discovery](docs/discovery.md) — how learning works and what evidence can support
- [Survey](docs/survey.md) — respondent-ready draft and pilot constraints
- [Design direction](docs/design.md) — durable Discovery Landing V1 direction
- [Writing and localization](docs/writing.md) — public voice and multilingual editorial guidance
- [Knowledge and evidence](docs/knowledge-and-evidence.md) — what is known and not known
- [Decisions](docs/decisions.md) — durable choices and rationale
- [Roadmap](docs/roadmap.md) — current position and evidence-gated sequence
- [Project instructions](AGENTS.md) — working, implementation, privacy, and safety guardrails
