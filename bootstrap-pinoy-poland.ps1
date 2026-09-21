[CmdletBinding()]
param(
    [string]$ProjectName = "Pinoy Poland",
    [switch]$Force
)

$ErrorActionPreference = "Stop"

$Root = (Get-Location).Path
$Docs = Join-Path $Root "docs"
$DecisionDate = Get-Date -Format "yyyy-MM-dd"

function Write-ProjectFile {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [string]$Content
    )

    if ((Test-Path -LiteralPath $Path) -and -not $Force) {
        Write-Host "Skipped existing file: $Path" -ForegroundColor Yellow
        return
    }

    $Directory = Split-Path -Parent $Path
    if ($Directory -and -not (Test-Path -LiteralPath $Directory)) {
        New-Item -ItemType Directory -Force -Path $Directory | Out-Null
    }

    $Utf8NoBom = New-Object System.Text.UTF8Encoding($false)
    [System.IO.File]::WriteAllText($Path, $Content + [Environment]::NewLine, $Utf8NoBom)
    Write-Host "Created: $Path" -ForegroundColor Green
}

$Agents = @'
# AGENTS.md

## Purpose

This repository explores a product that may make living and working in Poland easier for Filipinos.

`Pinoy Poland` is a working name, not a final branding decision.

The project is currently in product discovery.

Do not assume that the final product is an employment agency, job board, immigration service,
housing platform, administrative assistance service, community platform, or any other specific
business. These are possibilities to investigate, not commitments.

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

A second discovery track may later include Polish employers and relevant service providers.

## Problem areas to investigate

Potential areas include employment, changing employers, residence and work-related procedures,
understanding Polish documents, public administration, accommodation, banking, taxes, healthcare,
transportation, language, employment contracts, family-related needs, community, trustworthy
information, and preparing to move to Poland.

These are research areas, not confirmed product requirements.

## Current stage

Prefer partner review, open conversations, survey piloting, interviews, observation,
official-source research, documenting evidence, and testing willingness to use or pay.

Avoid premature application architecture, frameworks, databases, cloud infrastructure,
authentication, marketplaces, recruitment systems, payment systems, and mobile applications.

Technology should follow a validated need.

## Source of truth

- `AGENTS.md` — project guardrails
- `docs/vision.md` — current vision
- `docs/discovery.md` — hypotheses and discovery strategy
- `docs/survey.md` — current survey research outline
- `docs/knowledge-and-evidence.md` — verified knowledge and collected evidence
- `docs/decisions.md` — durable decisions
- `docs/roadmap.md` — current sequence of work

## Privacy and safety

Do not commit personally identifying respondent data, private messages, identity documents,
residence documents, employment documents, sensitive personal stories, or raw interview
recordings.

Store only appropriately anonymized or aggregated research evidence in the repository.

Legal, immigration, employment, tax, and administrative information must be verified against
appropriate authoritative sources before being presented as guidance.

Do not promise visas, jobs, residence outcomes, or administrative outcomes.

## Guiding rule

Discover what is worth building before deciding what to build.
'@

$Vision = @'
# Vision

## Working idea

`Pinoy Poland` is a working name for an early-stage project exploring how to make living and
working in Poland easier for Filipinos.

The final product and business model are deliberately undecided.

The project starts with people and their real experiences rather than with software.

## Human problem space

Potential questions include finding trustworthy work, understanding Polish documents, dealing
with offices, changing employers, finding accommodation, opening a bank account, and knowing
where to ask for reliable help.

For people considering Poland, discovery may cover whether Poland is a realistic option, how work
and preparation function, how to find trustworthy information, and what happens after arrival.

These examples describe the problem space. They are not validated product requirements.

## Possible future value

A future service might help a person describe their situation, identify what they need, understand
what to do next, find trusted sources, and know where to obtain further help.

Exactly which parts should become a product must be discovered.

## Possible directions

Evidence may eventually point toward information, practical guidance, administrative help, jobs,
housing, community, employer services, or something not yet discovered.

Do not treat these possibilities as a product roadmap.
'@

$Discovery = @'
# Discovery

## Objective

Discover which problems are real, repeated, important, and insufficiently solved before deciding
what product to build.

## Discovery loop

```text
Idea
 |
 v
Assumptions
 |
 v
Filipino partner review
 |
 v
Open conversations
 |
 v
Survey pilot
 |
 v
Broader survey + follow-up interviews
 |
 v
Evidence
 |
 v
Repeated problems + existing alternatives
 |
 v
Problem worth solving?
 |
 +-- no --> learn and continue discovery
 |
 +-- yes --> smallest experiment --> real usage --> willingness to pay
```

## Initial questions

Learn what problems Filipinos actually experience in Poland, which are most important, how they
describe those problems in their own words, how they solve them today, which alternatives work or
fail, whether people already pay for help, and which problems require information, human
assistance, regulated professionals, employers, or partners.

## First evidence sources

1. Review by the Filipino project partner to challenge framing, language, and obvious omissions.
2. A small number of open conversations with Filipinos before fixing survey categories.
3. A small survey pilot to test comprehension, trust, branching, and missing answer options.
4. A broader structured survey plus follow-up interviews.
5. Authoritative research where procedures or law matter.
6. Later, conversations with employers or relevant businesses.

Partner review is valuable context, not community validation. Do not confuse quantity of survey
responses with quality of evidence.

## Initial discovery bounds

A provisional first discovery round may aim for:

- about 50 completed surveys
- about 10 substantive Filipino conversations or follow-up interviews
- about 5 employer or relevant-business conversations

These numbers bound the initial learning effort; they are not validation thresholds, business
success metrics, or proof of demand. Adjust them if partner review, open conversations, or the
survey pilot shows that a different approach would produce better evidence.

Behavior and past experience are stronger evidence than hypothetical enthusiasm.
'@
$Survey = @'
# Survey

## Status

This document is a **research outline**, not yet a respondent-ready survey.

The outline should first be challenged by the Filipino project partner and informed by a small
number of open conversations. Only then should it become a concrete questionnaire with exact
wording, answer options, branching, and a pilot.

## Purpose

The eventual survey should help identify repeated problems, understand how people handle them
today, and recruit participants for deeper conversations. It should be short enough to complete
easily and should evolve based on what is learned.

The survey must not force respondents into categories created by the project team.

## Before fixing the questionnaire

Ask open questions in partner review and early conversations, such as:

- What has been unexpectedly difficult about living or working in Poland?
- Tell us about the last time you needed help with something in Poland.
- What did you do, and who or what helped you?
- What still feels confusing, risky, expensive, slow, or difficult?
- What do you wish you had known earlier?

Use the language and recurring situations from these conversations to refine later answer options.

## Draft introduction

We are exploring how to make living and working in Poland easier for Filipinos.

Before building anything, we want to understand real experiences, problems, and needs.

This short survey should take only a few minutes.

The wording is provisional until partner review and pilot testing.

## Draft branching

A future questionnaire may ask where the participant is now: Poland, Philippines, another country,
or previously in Poland.

For people in Poland, possible context includes how long they have lived there, where they live,
what brought them to Poland, and how they found work and accommodation.

Possible problem areas currently hypothesized by the project include work, employer changes, work
and residence documents, official letters, public offices, accommodation, banking, taxes,
healthcare, transportation, Polish language, employment contracts, family matters, trustworthy
information, and community.

Do not present this list before giving respondents an opportunity to describe important problems
in their own words. Always allow an `Other` or equivalent free-text route.

If a category list survives pilot testing, participants may be asked to identify a small number of
their most important problems rather than checking everything.

## Existing solutions and behavior

For important problems, ask about concrete past behavior: what happened the last time, what the
participant did, who or what they used, whether the problem was resolved, and what was difficult.

Ask whether they have ever paid for relevant help, what help they received, approximately how much
they paid, and whether it was useful.

Hypothetical willingness to pay is not proof of demand.

## Pilot before broader use

Test the questionnaire with a small number of people before broader distribution. Look for
confusing wording, missing options, leading questions, trust concerns, unnecessary questions, and
branching that does not match real situations.

Revise the survey from pilot evidence rather than treating the first draft as fixed.

## Follow-up and privacy

Ask whether the participant would be willing to talk for about 15 minutes about their experience.

Collect only contact information that is genuinely needed for follow-up, explain why it is being
requested, and keep it outside this repository.

After completion, participants may be invited to share the survey with another Filipino.

Never fabricate community percentages as if they were collected evidence. Publish only
appropriately aggregated and anonymized information.
'@
$Evidence = @'
# Knowledge and Evidence

## Purpose

This document separates what the project knows from what it currently assumes.

Do not promote assumptions into evidence without a supporting source or real observation.

## Current evidence

The project currently has an initial idea and personal observations that justify discovery.

No broad community need has yet been validated.
No product-market fit has been established.
No willingness to pay has been established.
No specific product direction has been selected.

## Current hypotheses

Hypotheses include practical information gaps, difficult administrative processes, employment
and accommodation problems, reliance on informal community help, value in trustworthy information,
possible demand for paid human assistance, and complementary employer problems.

None of these should be represented as validated market facts yet.

## Evidence entries

Add dated entries using:

```text
## YYYY-MM-DD — Short description

Source:
Observation:

What this supports:

What this does not prove:

Next question:
```

Keep personally identifying respondent information outside the repository.
'@

$Decisions = @"
# Decisions

## $DecisionDate — Start with discovery rather than implementation

Begin the project as a product-discovery repository. Do not select application architecture or
implement a product yet.

The problem space is broad and the most valuable problem has not been validated.

## $DecisionDate — Treat Pinoy Poland as a working name

Use **Pinoy Poland** as the current repository and project name without treating it as the final
brand.

## $DecisionDate — Use English as the initial project language

Keep initial repository documentation in English so it is understandable to Filipino and Polish
collaborators.

## $DecisionDate — Prefer compact text diagrams

Use small text diagrams for simple flows, hierarchies, and relationships when they improve
understanding.

## $DecisionDate — Keep personal research data out of Git

Do not store personally identifying survey or interview data in this repository.
"@

$Roadmap = @'
# Roadmap

## Current position

```text
Idea
 |
 v
Repository foundation
 |
 v
Filipino partner review       <- NEXT
 |
 v
Open conversations
 |
 v
Survey draft + pilot
 |
 v
Broader community discovery
 |
 v
Evidence
 |
 v
Problem worth solving
 |
 v
Smallest experiment
 |
 v
Real usage
 |
 v
Business validation
 |
 v
Product direction
```

## Stage 1 — Repository foundation

Status: **Current**

Establish project purpose, discovery guardrails, initial vision, hypotheses, research outline,
evidence model, durable decisions, and roadmap.

## Stage 2 — Filipino partner review

Status: **Next**

Review the project from the Filipino partner perspective. Check whether the problem space resembles
real experience, which assumptions look wrong, what is missing, how people may naturally describe
their situations, and whether the proposed research approach would feel understandable and
trustworthy.

Record useful observations as input to discovery, while keeping clear that one partner perspective
is not community validation.

## Stage 3 — Open conversations

Status: **Planned**

Have a small number of open conversations with Filipinos before fixing questionnaire categories.
Use concrete past experiences and participants' own language to challenge the current hypotheses
and improve the research outline.

## Stage 4 — Survey draft and pilot

Status: **Planned**

Turn the research outline into a respondent-ready questionnaire only after the earlier qualitative
learning. Pilot it with a small number of people and refine wording, answer options, branching,
privacy expectations, and missing areas.

Use an existing survey tool if it is sufficient; custom software is not required for discovery.

## Stage 5 — First community discovery

Status: **Planned**

Run a bounded first discovery round. A provisional target is about 50 survey responses and about
10 substantive Filipino conversations or follow-up interviews, but adjust the bounds if earlier
learning indicates a better approach.

## Stage 6 — Employer discovery

Status: **Planned**

Talk to a small number of employers or relevant businesses. A provisional target is about 5
conversations, subject to change as discovery develops.

## Stage 7 — Select a problem

Status: **Blocked by evidence**

Choose a first problem only when discovery provides sufficient evidence.

## Stage 8 — Smallest useful solution

Status: **Blocked**

Only after selecting a problem should the project choose technology and implementation.

The first solution may be software, a manual service, content, a workflow, a partnership, or
something else.

## Guiding rule

Do not ask: "What application should we build?"

Ask: "What have we learned, and what is the smallest experiment that should happen next?"
'@
$Readme = @'
# __PROJECT_NAME__

An early-stage project exploring how to make living and working in Poland easier for Filipinos.

**__PROJECT_NAME__ is a working name.**

The project is currently in **discovery**. We are deliberately starting with people, problems,
and evidence before deciding what product or business to build.

## Current approach

```text
Listen
 |
 v
Partner review + open conversations
 |
 v
Survey pilot
 |
 v
Broader discovery
 |
 v
Evidence
 |
 v
Choose one real problem
 |
 v
Test the smallest useful solution
```

We are not yet building an employment agency, housing marketplace, immigration service, or large
application. Evidence should determine what comes next.

## Repository structure

```text
pinoy-poland/
|-- AGENTS.md
|-- README.md
|-- bootstrap-pinoy-poland.ps1
`-- docs/
    |-- vision.md
    |-- discovery.md
    |-- survey.md
    |-- knowledge-and-evidence.md
    |-- decisions.md
    `-- roadmap.md
```

## Current status

Repository foundation and the first discovery model are being established.

The next meaningful step is reviewing the idea and research outline from the Filipino partner
perspective, followed by a few open conversations before finalizing a survey.
'@
$Readme = $Readme.Replace('__PROJECT_NAME__', $ProjectName)

$Files = @{
    (Join-Path $Root "AGENTS.md") = $Agents
    (Join-Path $Root "README.md") = $Readme
    (Join-Path $Docs "vision.md") = $Vision
    (Join-Path $Docs "discovery.md") = $Discovery
    (Join-Path $Docs "survey.md") = $Survey
    (Join-Path $Docs "knowledge-and-evidence.md") = $Evidence
    (Join-Path $Docs "decisions.md") = $Decisions
    (Join-Path $Docs "roadmap.md") = $Roadmap
}

New-Item -ItemType Directory -Force -Path $Docs | Out-Null

foreach ($Entry in $Files.GetEnumerator()) {
    Write-ProjectFile -Path $Entry.Key -Content $Entry.Value
}

Write-Host ""
Write-Host "Product discovery bootstrap complete." -ForegroundColor Cyan
Write-Host "Project: $ProjectName"
Write-Host ""
Write-Host "Review the generated files before committing." -ForegroundColor Cyan

if (Get-Command git -ErrorAction SilentlyContinue) {
    Write-Host ""
    git status --short
}
