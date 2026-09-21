[CmdletBinding()]
param(
    [string]$ProjectName = "Pinoy Poland",
    [switch]$Force
)

$ErrorActionPreference = "Stop"

$Root = (Get-Location).Path
$Docs = Join-Path $Root "docs"

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
    [System.IO.File]::WriteAllText($Path, $Content + "`n", $Utf8NoBom)
    Write-Host "Created: $Path" -ForegroundColor Green
}

$Agents = @'
# AGENTS.md

## Purpose

This repository explores a simple practical-help website that may make living and working in Poland
easier for Filipinos.

`Pinoy Poland` is a working name, not a final branding decision. The project remains in product
discovery, and its first useful capability is not yet selected.

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

A second discovery track may later include Polish employers and relevant service providers when
evidence makes that useful.

## Problem areas to investigate

Potential areas include work, finding and changing jobs, employment questions, documents, PESEL,
residence and work-related procedures, Polish letters and public offices, accommodation, banking,
taxes, healthcare, transportation, language, employment contracts, family-related needs,
community, trustworthy information, everyday life, and preparing to move to Poland.

These are research areas, not confirmed needs or product requirements.

## Current stage

The immediate working artifact is a short, respondent-ready survey. Share it first with a few
Filipinos, learn from the pilot, improve it, and only then distribute it more broadly. Use aggregate
patterns and useful follow-up conversations to decide what deserves deeper investigation.

Natural Filipino feedback should challenge assumptions, but no partner or individual is a formal
review gate or community representative. The survey is a learning mechanism, not proof of demand
or validation by itself.

Prefer survey piloting, conversations, observation, official-source research, documenting evidence,
and later tests of real use or payment. Avoid premature application architecture, frameworks,
databases, cloud infrastructure, authentication, marketplaces, recruitment systems, payment
systems, and mobile applications. Technology should follow a validated need.

## Source of truth

- `AGENTS.md` __EM_DASH__ project guardrails
- `docs/vision.md` __EM_DASH__ current vision
- `docs/discovery.md` __EM_DASH__ hypotheses and discovery strategy
- `docs/survey.md` __EM_DASH__ respondent-ready survey draft and pilot plan
- `docs/knowledge-and-evidence.md` __EM_DASH__ decisions, hypotheses, and collected evidence
- `docs/decisions.md` __EM_DASH__ durable decisions
- `docs/roadmap.md` __EM_DASH__ current sequence of work

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
'@

$Readme = @'
# __PROJECT_NAME__

### A simple idea to make life in Poland easier for Filipinos

```text
              LIVING IN POLAND
                     |
        Sometimes you just need help.
                     |
     +---------------+---------------+
     |               |               |
    WORK          DOCUMENTS      DAILY LIFE
     |               |               |
 finding jobs     PESEL           housing
 changing jobs    residence       banking
 contracts        work permits    healthcare
 employers        Polish letters  taxes
     |               |               |
     +---------------+---------------+
                     |
              WHERE DO I START?
```

## The idea

Create a simple website for Filipinos in Poland where you can find practical help with real
everyday problems.

Not complicated information.

Just:

```text
I HAVE A PROBLEM
       |
       v
What should I do?
       |
       v
Where should I go?
       |
       v
What do I need?
       |
       v
Simple explanation
       |
       v
Trusted information / useful help
```

But instead of guessing what Filipinos need, we start by asking them.

## How we start

```text
        SHORT SURVEY
             |
             v
 Share it with a few Filipinos
             |
             v
     They may share it further
             |
             v
   We collect real answers
             |
             v
       SEE THE RESULTS
             |
             v
 What problems appear most often?
             |
             v
        START WITH ONE
             |
             v
     Build something useful
             |
             v
      See if people use it
             |
             v
     Learn -> improve -> repeat
```

So maybe the first thing people need is help with jobs.

Maybe it's documents.

Maybe housing.

Maybe understanding Polish offices and procedures.

Or maybe it's something we haven't thought about at all.

The survey helps us find out. It is a starting point for learning, not proof by itself.

And later the website could grow naturally:

```text
                  PINOY POLAND
                       |
          +------------+------------+
          |            |            |
        HELP          JOBS         LIFE
          |            |            |
      Documents      Offers       Housing
      Procedures     Employers    Banking
      Offices        Changing     Healthcare
      Letters        jobs         Everyday help
          |            |            |
          +------------+------------+
                       |
                   COMMUNITY
```

These are possible future directions, not a promised feature list or product structure. What we
learn may point somewhere different.

Start small. Ask people. See the real problems. Build what is actually useful.

Something made with Filipinos in Poland, not just for Filipinos in Poland.

## About this repository

__PROJECT_NAME__ is a working name. This repository holds the project's discovery notes and keeps the
work evidence-first. No first product capability has been chosen yet.

For contributors:

- [Vision](docs/vision.md)
- [Discovery approach](docs/discovery.md)
- [Survey draft](docs/survey.md)
- [Knowledge and evidence](docs/knowledge-and-evidence.md)
- [Decisions](docs/decisions.md)
- [Roadmap](docs/roadmap.md)
- [Project instructions](AGENTS.md)
'@

$Vision = @'
# Vision

## Human goal

Make living and working in Poland easier for Filipinos.

`Pinoy Poland` is a working name. The current working product concept is a simple website where
Filipinos in Poland could find practical help with real everyday problems.

It could help a person move from:

```text
I have a problem
       |
       v
What should I do, where should I go, and what do I need?
       |
       v
Simple explanation and trusted information or useful help
```

## Possible problem areas

Discovery may find important problems involving work, jobs, changing employers, documents, PESEL,
residence and work procedures, Polish letters and offices, accommodation, banking, healthcare,
taxes, transportation, language, everyday life, trustworthy information, community, or something
not yet considered.

These are examples and hypotheses, not validated needs or committed product areas.

## Current product position

The practical-help website is a clear working concept, but its first useful capability remains
undecided. A short survey, followed by useful conversations, will look for repeated concrete
problems and how people handle them today.

Evidence should determine which one problem is worth testing first and whether the smallest useful
solution is information, a service, a workflow, software, a partnership, or something else.

The business model, technology, and broader product direction remain evidence-gated.
'@

$Discovery = @'
# Discovery

## Objective

Discover which problems are real, repeated, important, and insufficiently solved before choosing
the first product capability.

## Discovery loop

```text
Idea
 |
 v
Simple explanation
 |
 v
Short survey
 |
 v
Small initial sharing / pilot
 |
 v
Learn and improve
 |
 v
Broader sharing
 |
 v
Responses + follow-up conversations
 |
 v
Patterns / repeated problems
 |
 v
Problem worth testing?
 |
 +-- no --> continue learning
 |
 +-- yes
        |
        v
Smallest useful solution
        |
        v
Real usage
        |
        v
Learn -> improve -> repeat
```

The survey is the first practical discovery mechanism. It is not proof of demand, product
validation by itself, a product requirement generator, or a substitute for conversations.

## What we want to learn

We want to learn what people have found difficult, what happened in a recent real situation, how
they tried to solve it, which alternatives they used, whether it was resolved, and what they wish
had been easier. Past behavior and repeated concrete experiences are stronger evidence than
hypothetical enthusiasm.

The survey should allow people to describe problems in their own words before offering categories
for analysis. Counts can reveal patterns worth investigating, but counts alone do not prove demand.
Follow-up conversations can explain why a pattern exists and what happened in real situations.

## Evidence discipline

- Assumptions are not evidence.
- Survey responses are evidence, but not automatic product requirements.
- One person's experience is useful context, not validation for a community.
- Existing alternatives, actual behavior, unresolved outcomes, and real usage matter.
- Hypothetical willingness to use or pay is not proof of demand.
- Natural comments and corrections from the Filipino project partner or anyone else may inform
  discovery, but no individual is a formal review gate or community representative.
- Legal, residence, employment, tax, and administrative facts must be checked against appropriate
  authoritative sources before they are presented as guidance.

## Initial discovery bounds

A provisional first discovery round may aim for about 50 completed surveys and about 10 substantive
Filipino conversations or follow-ups. A later employer track may include about 5 conversations if
the emerging evidence makes that useful.

These figures only bound planning effort. They are not targets that prove success, validation
thresholds, or evidence of demand, and they may change as the pilot teaches us more.
'@

$Survey = @'
# Survey

## Status

This is the project's immediate working artifact: a respondent-ready draft for a small pilot. It
has not been piloted or broadly shared yet. The pilot should improve it before wider distribution.

Use an existing survey tool if it is sufficient. Keep the form short, phone-friendly, friendly,
and easy to share. Exact answer options and branching may be adjusted in the survey tool after the
pilot, with meaningful learning recorded here.

## Purpose

The survey should help us notice repeated problems, understand how people currently handle them,
and find volunteers for useful follow-up conversations. It is a learning mechanism, not proof of
demand or a generator of product requirements.

## Respondent-ready draft

### Introduction

**Help us understand what could make life in Poland easier for Filipinos.**

We are exploring a simple website with practical help for Filipinos in Poland. Before building
anything, we want to learn about real experiences and problems.

This short survey should take about 5 minutes. You can skip any optional question. Please do not
share identity or residence documents, document numbers, or other sensitive personal information.
We will look at answers together as aggregate patterns, not publish personal stories.

### Questions

1. **Which best describes you now?**
   - Living or working in Poland
   - Previously lived or worked in Poland
   - Considering moving to Poland
   - Other

2. **In your own words, what has been difficult about living, working, or preparing to move to
   Poland?** *(Optional free text)*

3. **Think about the last time you needed help while living, working, or preparing to move to
   Poland. What happened?**
   - Describe what happened: ______
   - I have not had an experience like this yet

   Do not include names, document numbers, or sensitive details. If the respondent has not had
   such an experience, skip Questions 4-5 and continue with Question 6.

4. **What did you do to solve it, and who or what helped you?**
   *(Shown only after a concrete experience; optional free text.)*

5. **Was the problem resolved?** *(Shown only after a concrete experience.)*
   - Yes, fully
   - Partly
   - No
   - It is still in progress
   - Prefer not to say

6. **Where do you usually look for help or information?** *(Select all that apply.)*
   - Friends or family
   - Filipino community or social media groups
   - Employer or recruitment agency
   - Polish government or official websites/offices
   - Search engines or other websites
   - Lawyer, accountant, adviser, or another paid professional
   - Nonprofit or community organization
   - I am not sure where to look
   - Other

7. **Which areas have been difficult or confusing for you?** *(Select up to three. These are
   possible areas, not assumed needs.)*
   - Work or finding a job
   - Changing jobs or employers
   - Employment contracts
   - Documents, PESEL, or Polish letters
   - Residence or work-related procedures
   - Polish offices and public administration
   - Accommodation
   - Banking
   - Taxes
   - Healthcare
   - Transportation
   - Polish language
   - Family matters
   - Finding trustworthy information
   - Community or meeting people
   - None of these
   - Other: ______

8. **What do you wish had been easier, or what do you wish you had known earlier?**
   *(Optional free text)*

9. **Have you ever paid for help with one of these situations?**
   - Yes
   - No
   - Prefer not to say

   If yes, optionally ask what kind of help they paid for and whether it was useful. Do not ask for
   exact financial details during the pilot unless there is a clear reason.

10. **Would you be willing to have a short follow-up conversation about your experience?**
    - Yes
    - Maybe
    - No

    If yes or maybe, the survey tool may request an optional contact method with a clear explanation
    of why it is needed. Store contact details outside Git and separately from shared analysis.

### Closing

Thank you. Your answers will help us decide what deserves deeper investigation before anything is
built. If you know another Filipino whose experience could help, you may share this survey with
them.

## Pilot plan

Share the draft with a few Filipinos first. Natural feedback from the Filipino project partner or
anyone else is welcome, but no one person is a formal reviewer or validation gate.

Observe or ask:

- Does the introduction feel natural and trustworthy?
- Is the reason for the survey clear?
- Is it short and easy to complete on a phone?
- Are any questions confusing, leading, intrusive, or unnecessary?
- Are important answer options missing?
- Does the order and any branching make sense?
- Do the answers reveal real past behavior and unresolved problems?
- Would the answers help decide what deserves deeper investigation?
- Do people naturally feel comfortable sharing it further?

Revise the survey from pilot evidence before broader sharing. Do not treat the pilot as completed
until it has actually happened.

## Analysis and privacy

Later analysis may report honest, anonymized aggregate observations about recurring categories,
current ways of finding help, unresolved problems, and differences between respondent situations.
Do not invent percentages or treat any response count as automatic proof of demand.

Do not collect identity documents, residence documents, document numbers, unnecessary legal or
personal details, raw private stories, or information without a clear discovery purpose. Keep all
respondent-level and contact data outside the repository.

Hypothetical willingness to use or pay is not proof. Follow-up conversations and eventual real
usage are needed to understand what survey patterns mean.
'@

$Evidence = @'
# Knowledge and Evidence

## Purpose

This document separates project decisions, hypotheses, and collected evidence. Decisions about
how to learn are not market evidence.

## What we have decided

- `Pinoy Poland` remains a working name.
- The current working concept is a practical-help website for Filipinos in Poland.
- Discovery starts with a short, respondent-ready survey shared first with a few people.
- The survey will be improved from the pilot before broader sharing.
- Aggregate results and useful follow-up conversations will be used to investigate repeated
  problems and how people currently solve them.
- Evidence should determine the first problem to test.
- A smallest-useful-solution experiment should come before any large platform.

These are project and process decisions. They do not show that a market need exists.

## What remains hypothetical

Potential needs may involve work, finding or changing jobs, documents, residence and work
procedures, Polish letters and public offices, accommodation, banking, taxes, healthcare,
transportation, language, contracts, family matters, trustworthy information, community,
everyday life, or problems discovery has not yet revealed.

None of these areas is a confirmed need, priority, feature, or product requirement.

## Evidence we do not yet have

No survey has been run and no community statistics currently exist. The project does not yet have:

- evidence of broad community need or validated demand;
- a validated problem ranking or first feature;
- product-market fit or usage evidence;
- demonstrated willingness to pay;
- a validated business model; or
- evidence of employer demand.

## Evidence entries

Add appropriately anonymized or aggregated evidence using:

```text
## YYYY-MM-DD __EM_DASH__ Short description

Source:
Observation:

What this supports:

What this does not prove:

Next question:
```

Keep personally identifying respondent information, contact details, private messages, sensitive
stories, documents, and raw recordings outside this repository.
'@

$Decisions = @'
# Decisions

## 2026-09-21 __EM_DASH__ Start with discovery rather than implementation

Begin the project as a product-discovery repository. Do not select application architecture or
implement a product yet.

The problem space is broad and the most valuable problem has not been validated.

## 2026-09-21 __EM_DASH__ Treat Pinoy Poland as a working name

Use **Pinoy Poland** as the current repository and project name without treating it as the final
brand.

## 2026-09-21 __EM_DASH__ Use English as the initial project language

Keep initial repository documentation in English so it is understandable to Filipino and Polish
collaborators.

## 2026-09-21 __EM_DASH__ Prefer compact text diagrams

Use small text diagrams for simple flows, hierarchies, and relationships when they improve
understanding.

## 2026-09-21 __EM_DASH__ Keep personal research data out of Git

Do not store personally identifying survey or interview data in this repository.

## 2026-09-21 __EM_DASH__ Explain the working idea simply and accessibly

Use a short, visual, human explanation as the preferred external introduction at this stage: a
simple practical-help website could make everyday problems easier for Filipinos in Poland, and we
will ask people before choosing its first capability.

Possible work, document, daily-life, and community directions remain exploratory rather than a
committed product structure.

## 2026-09-21 __EM_DASH__ Start practical discovery with a short survey and small pilot

The initial foundation placed formal Filipino partner review and open conversations before survey
work. That sequence usefully emphasized Filipino perspectives, but treating one partner as a
required reviewer created unnecessary formality and pressure.

Natural reactions, corrections, experiences, and ideas from the Filipino project partner or
anyone else remain valuable discovery input. No individual is a validation gate or represents the
wider Filipino community.

The first practical mechanism is now a short survey shared initially with a few Filipinos. Improve
it from the pilot before broader sharing, then use aggregate patterns and useful follow-up
conversations to decide what deserves deeper investigation. Survey responses are evidence, but the
survey is not validation by itself and does not automatically create product requirements.
'@

$Roadmap = @'
# Roadmap

## Current position

```text
Repository foundation
        |
        v
Survey draft                <- CURRENT / NEXT WORK
        |
        v
Small pilot
        |
        v
Learn + improve survey
        |
        v
Broader sharing
        |
        v
Responses + conversations
        |
        v
Patterns / evidence
        |
        v
Choose one problem
        |
        v
Smallest useful solution
        |
        v
Real usage
        |
        v
Learn + iterate
```

## Stage 1 __EM_DASH__ Repository foundation

Status: **Complete**

The project purpose, discovery guardrails, working vision, evidence model, durable decisions, and
roadmap are established. They will continue to evolve as evidence appears.

## Stage 2 __EM_DASH__ Respondent-ready survey

Status: **Current**

Review the short draft as a real form, choose an existing survey tool, and prepare it for a small
pilot. Keep it natural, phone-friendly, privacy-conscious, and focused on real experiences rather
than hypothetical product enthusiasm.

## Stage 3 __EM_DASH__ Small pilot

Status: **Next**

Share the survey with a few Filipinos. Learn whether it is clear, trustworthy, short enough,
non-leading, and capable of producing answers useful for the next discovery decision.

## Stage 4 __EM_DASH__ Learn and improve

Status: **Planned**

Review initial responses and feedback, then improve wording, categories, options, and branching.
Do not treat the pilot as validation or a completed discovery round.

## Stage 5 __EM_DASH__ Broader sharing

Status: **Planned**

Share the improved survey more broadly. People may share it further. Keep respondent data outside
Git and record only appropriately anonymized or aggregated evidence here.

## Stage 6 __EM_DASH__ Patterns and conversations

Status: **Planned**

Analyze aggregate patterns, including repeated problems, existing alternatives, and unresolved
outcomes. Use follow-up conversations where they can explain what happened and why. Planning
bounds may guide effort, but no response count is a validation threshold.

## Stage 7 __EM_DASH__ Select one problem

Status: **Blocked by evidence**

Choose a first problem only when the evidence justifies testing it. Employer or service-provider
discovery may be added if emerging evidence makes it relevant.

## Stage 8 __EM_DASH__ Smallest useful solution

Status: **Blocked by evidence**

Test the smallest useful response to the selected problem, then observe real usage and iterate.
The response may be content, a manual service, a workflow, a partnership, software, or something
else. Technology and business decisions come after the problem is understood.

## Guiding rule

Do not ask: "What application should we build?"

Ask: "What have we learned, and what is the smallest useful experiment that should happen next?"
'@

$TemplateVariables = @('Agents', 'Readme', 'Vision', 'Discovery', 'Survey', 'Evidence', 'Decisions', 'Roadmap')
foreach ($VariableName in $TemplateVariables) {
    $Value = Get-Variable -Name $VariableName -ValueOnly
    Set-Variable -Name $VariableName -Value $Value.Replace('__EM_DASH__', [string][char]0x2014)
}

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

if ((Get-Command git -ErrorAction SilentlyContinue) -and (Test-Path -LiteralPath (Join-Path $Root ".git"))) {
    Write-Host ""
    git status --short
}
