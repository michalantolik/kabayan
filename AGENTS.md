# AGENTS.md

## Purpose

This repository explores a simple practical-help website that may make living and working in Poland
easier for Filipinos.

**Kabayan** is the project and repository identity. **Kabayan Poland** is the chosen public name for the initial Poland market, with `kabayanpoland.com` as the intended primary public domain once live availability is confirmed and the domain is secured. This naming decision replaces the earlier `Pinoy Poland` working name; it does not validate a product, market need, or business model.

Poland is the only current discovery market. Germany, Austria, Switzerland, or other markets may be explored later only if evidence supports expansion. Do not implement multi-market functionality merely because expansion is plausible.

Do not assume that the final product is an employment agency, job board, immigration service, housing platform, administrative assistance service, community platform, or any other specific business. These are possibilities to investigate, not commitments.

## Core principle

Prefer evidence over assumptions.

```text
Assumption
    |
    v
Experiment
    |
    v
Evidence
    |
    v
Decision
    |
    v
Smallest useful change
```

Do not turn an interesting idea into a validated user need.
Do not turn one person's experience into evidence about the entire Filipino community.
Do not turn survey responses into product requirements without sufficient evidence.
Do not build software merely because software can be built.

## Initial audience

Initial discovery may include Filipinos living or working in Poland, people who previously lived
or worked in Poland, and Filipinos considering moving to Poland.

A second discovery track may later include Polish employers and relevant service providers when
evidence makes that useful.

## Problem areas to investigate

Potential areas include work, finding and changing jobs, employment questions, documents, PESEL,
residence and work-related procedures, Polish letters and public offices, accommodation, banking,
taxes, healthcare, transportation, language, employment contracts, family-related needs,
community, trustworthy information, everyday life, and preparing to move to Poland.

These are research areas, not confirmed needs or product requirements.

## Current stage

The product/discovery roadmap remains at **Stage 2 — Discovery preparation**. The survey foundation
is respondent-ready but has not been piloted. A responsive local Discovery Landing V1 prototype
has completed Michał's visual and product review and the resulting bounded responsive and copy
refinement. `Kabayan.Web` now owns an implementation migrated from that prototype, while the
committed static files remain the comparison baseline for this iteration. A GitHub Pages workflow
exists and still publishes the static landing, not `Kabayan.Web`. Do not infer from the workflow
alone that a public deployment has run or that the custom domain is configured. Neither landing
collects responses.

Launchpad Application Foundation is installed and validated. The repository contains
`Kabayan.Web`, `Kabayan.Api`, `Kabayan.Application`, and `Kabayan.Infrastructure` with corresponding
test projects. `Kabayan.Web` preserves the generic Foundation capabilities and now uses the
Discovery Landing as its anonymous entry experience. Localization 1.0 is not installed; the
landing keeps its bounded English, draft Filipino, and Polish content within the Web feature.
Foundation installation is technical
progress, not evidence that a product problem, capability, business model, or application
direction has been validated.

The next evidence-producing step is Pilot 0 locally with Gladys. The proposed origin material and
draft Filipino copy remain review material, not approved public content; Filipino copy has not
received human language review, and Gladys has not yet confirmed publication of her name,
relationship context, any future photograph, or attributed role. Pilot 0 is a first reality check
from one person close to the project, not representative evidence about Filipinos or a
public-deployment gate by itself. See `docs/roadmap.md` for the sequence and `docs/discovery.md` for
the learning method.

Natural Filipino feedback should challenge assumptions, but no partner or individual is a formal
review gate or community representative. The survey is a learning mechanism, not proof of demand
or validation by itself.

Prefer survey piloting, conversations, observation, official-source research, documenting evidence,
and later tests of real use or payment. The installed Foundation's generic authentication,
accounts, persistence, and application structure must not become assumed Kabayan product
requirements. Do not remove Foundation code merely because those capabilities are not yet
product-validated, and do not build product features on top of them until discovery evidence
creates a concrete need. Avoid additional premature frameworks, cloud infrastructure,
marketplaces, recruitment systems, payment systems, mobile applications, and speculative
abstractions. Technology should follow a validated need.


## Implementation simplicity and future-market safety

Build for Poland now. Keep plausible future expansion visible without implementing future requirements.

Prefer the simplest design that solves the current problem and does not unnecessarily lock the project to Poland. Do not introduce multi-market abstractions, country strategies, multi-tenancy, provider hierarchies, speculative frameworks, or pattern-heavy architecture until a real second case or another concrete present need justifies them.

When implementing a meaningful change, make this small safety check:

1. Is this the simplest good solution for the current Poland requirement?
2. Does it unnecessarily make a future second market difficult?
3. Are we adding an abstraction that has no concrete use today?

Prefer **yes / no / no**. Keep brand, market-specific content, and similar values in an obvious place when doing so is naturally simpler than scattering them, but do not build a configuration platform for one value. Generalize after a real second case appears.

## Source of truth

- `AGENTS.md` — project guardrails
- `docs/vision.md` — current vision
- `docs/brand-and-market-strategy.md` — naming, domain, market-expansion, and implementation-extension boundaries
- `docs/discovery.md` — hypotheses and discovery strategy
- `docs/survey.md` — respondent-ready survey draft and pilot plan
- `docs/design.md` — durable Discovery Landing V1 design direction
- `docs/writing.md` — public voice and EN / Filipino / Polish editorial guidance
- `docs/knowledge-and-evidence.md` — decisions, hypotheses, and collected evidence
- `docs/decisions.md` — durable decisions
- `docs/roadmap.md` — current sequence of work

## Privacy and safety

Do not commit personally identifying respondent data, contact details, private messages, identity
documents, residence documents, employment documents, sensitive personal stories, or raw interview
recordings.

Store only appropriately anonymized or aggregated research evidence in the repository.

Legal, immigration, employment, tax, residence, and administrative information must be verified
against appropriate authoritative sources before being presented as guidance.

Do not promise visas, jobs, residence outcomes, or administrative outcomes.

## Guiding rule

Discover what is worth building before deciding what to build.
