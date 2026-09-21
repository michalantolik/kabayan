# Roadmap

## Current position

```text
Foundation
    |
    v
Survey foundation
    |
    v
Discovery Landing V1 direction      <- ALIGNED
    |
    v
Responsive prototype implementation <- CURRENT / NEXT PACKAGE
    |
    v
Responsive validation
    |
    v
Pilot 0: trusted first review
    |
    v
Learn + refine
    |
    v
Small Filipino pilot
    |
    v
Broader discovery -> patterns -> choose one problem
    |
    v
Smallest useful solution -> real usage -> learn + iterate
```

The roadmap uses **Stage -> Step -> Package** only where it clarifies work. A stage is a meaningful
period of learning, a step is an outcome within it, and a package is the smallest bounded unit that
can be implemented, reviewed, validated, and closed independently. Implementation details below a
package do not belong here. Later stages stay coarse until evidence justifies more detail.

## Stage 1 — Foundation

Status: **Complete**

### Step: Establish discovery foundations

The purpose, evidence rules, privacy boundaries, vision, evidence model, respondent-ready survey
foundation, decisions, and initial roadmap exist.

## Stage 2 — Discovery preparation

Status: **Current**

### Step: Align Discovery Landing V1 direction

Status: **Complete — this documentation checkpoint**

This checkpoint defines the prototype's role, public narrative, design character, naming
exploration, writing and localization standard, evidence limits, Pilot 0, and relationship to the
survey without implementing it.

### Step: Prepare a realistic discovery experience

Status: **Current**

- **Package — Current / next: Implement the responsive Discovery Landing V1.** Build only the
  agreed public discovery experience, use `docs/writing.md` for its EN / Filipino / Polish copy,
  and connect it to an existing survey tool; do not imply unbuilt product capabilities. Begin
  locally. The minimum proposed origin narrative may be included as review material, without a
  photograph; local review is not publication approval.
- **Package — Planned: Validate responsive behavior and the complete participation journey.**
  Check phone and larger-screen comprehension, accessibility, actions, language behavior, and
  survey access.

### Step: Pilot 0 and refine

Status: **Planned**

- **Package: Run Pilot 0 locally with Gladys.** Let her review the actual page, including the
  proposed origin material, for spontaneous understanding, language, cultural context, trust, and
  personal presentation. This is not community validation or public approval by default.
- **Package: Apply one deliberate refinement and confirm publishable personal elements.** Refine
  the prototype and survey where the review supports it, then explicitly confirm which personal
  elements may be public.
- **Package: Publish the approved prototype to GitHub Pages.** Treat GitHub Pages as public and
  include only personal elements that Gladys has explicitly approved.

## Stage 3 — Small Filipino pilot

Status: **Planned**

Share the refined experience with a few Filipinos. Observe understanding and participation,
collect survey evidence and useful naming reactions, and improve before broader sharing. Neither
positive design feedback nor name preference validates a product need.

## Stage 4 — Broader discovery

Status: **Evidence-gated**

Share more broadly only after the small pilot supports doing so. Use aggregate responses,
observation, and useful conversations to investigate repeated problems, current alternatives, and
unresolved outcomes. Keep respondent-level data outside Git.

## Stage 5 — Problem selection

Status: **Blocked by evidence**

Choose one problem only when repeated evidence justifies deeper investigation and a realistic test.
Add employer or service-provider discovery only if emerging evidence makes it relevant.

## Stage 6 — First useful solution

Status: **Blocked by evidence**

Test the smallest useful response to the selected problem and observe real use. The response may be
information, a manual service, a workflow, a partnership, software, or something else. Do not
select architecture, infrastructure, or a business model before the problem and experiment require
them.

## Stage 7 — Learn and iterate

Status: **Blocked by evidence**

Use real usage and outcomes to improve, change direction, stop, or repeat the discovery loop.

## Guiding rule

Do not ask: "What application should we build?"

Ask: "What have we learned, and what is the smallest useful experiment that should happen next?"
