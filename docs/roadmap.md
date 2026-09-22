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
Responsive prototype implementation <- IMPLEMENTED
    |
    v
Initial responsive validation       <- COMPLETE
    |
    v
Michał visual + product review      <- COMPLETE
    |
    v
Bounded responsive + copy refinement <- COMPLETE
    |
    v
Kabayan naming + prototype alignment <- COMPLETE
    |
    v
Web landing migration                <- COMPLETE
    |
    v
Pilot 0: trusted first review       <- CURRENT / NEXT
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

This checkpoint defines the prototype's role, public narrative, design character, naming direction, writing and localization standard, evidence limits, Pilot 0, and relationship to the
survey without implementing it.

### Step: Prepare a realistic discovery experience

Status: **Complete — committed prototype present in supplied repository snapshot**

- **Package — Complete: Implement the responsive Discovery Landing V1 locally.** The dependency-free
  static prototype contains complete EN / draft Filipino / PL versions, the minimum proposed
  origin narrative without a photograph, the discovery loop, an honest survey handoff, and
  the discovery experience. It sends and stores no responses. The repository also contains a GitHub Pages workflow; its presence does not by itself prove that deployment has run.
- **Package — Complete: Perform initial responsive and functional validation.** The prototype has
  been rendered at representative phone, tablet, and desktop widths; language switching, internal
  actions, the then-current naming interaction, overflow, and local resources have been checked. This
  technical pass is not audience evidence or product approval.

### Step: Align the chosen Kabayan name before Pilot 0

Status: **Complete**

- **Package — Complete: Replace candidate-name exploration with Kabayan Poland.** Candidate-name voting/preview UI has been removed; public copy and repository documentation use the chosen name while preserving rationale and unresolved risks.

### Step: Move the discovery experience into the Web foundation

Status: **Complete**

- **Package — Complete: Migrate Discovery Landing V1 into `Kabayan.Web`.** The Web root now
  reproduces the static prototype's content, language switching, in-page actions, and responsive
  presentation. The static prototype remains the explicit comparison baseline, and the existing
  GitHub Pages workflow continues to publish it. No survey collection, backend behavior, or
  product capability was added.

### Step: Secure the intended public identity

Status: **Planned before relying on the public identity; not a gate for local Pilot 0**

- **Package — Planned: Confirm and secure the intended public identity.** Before relying on the domain or handles, confirm live availability and, if appropriate, register `kabayanpoland.com`; check/secure useful social handles. Consider `kabayanpoland.pl` and optionally `kabayan-poland.com` only as defensive redirects if their cost is justified. Formal trademark clearance is required before meaningful commercial brand investment, not as evidence that a product need exists. This work does not block local Pilot 0.

### Step: Pilot 0 and refine

Status: **Current — Pilot 0 next**

- **Package — Complete locally: Review and refine the prototype with Michał.** The visual and
  product review and its bounded responsive and copy refinement are complete. This produced no
  respondent evidence and did not make the prototype ready for public deployment.
- **Package — Current / next: Run Pilot 0 locally with Gladys.** Let her review the actual page,
  including the proposed origin material and draft Filipino copy, for spontaneous understanding,
  language, cultural context, trust, personal presentation, and spontaneous expectations created by the **Kabayan Poland** name. Do not ask her to choose among candidate names; observe whether the chosen name feels natural or misleading, especially whether it suggests recruitment. Filipino public copy has not yet
  received human review, and no personal element involving Gladys is approved for publication yet.
  This is a first reality check from one person close to the project, not representative evidence
  about Filipinos or public approval by default.
- **Package: Apply one deliberate refinement and confirm publishable personal elements.** Refine
  the prototype and survey where the review supports it, then explicitly confirm which personal
  elements may be public.
- **Package: Publish the approved prototype to GitHub Pages.** Treat GitHub Pages as public and
  include only personal elements that Gladys has explicitly approved.

## Stage 3 — Small Filipino pilot

Status: **Planned**

Share the refined experience with a few Filipinos. Observe understanding and participation,
collect survey evidence and useful spontaneous reactions to **Kabayan Poland**, and improve before broader sharing. Do not turn this into a candidate-name vote. Neither positive design feedback nor a positive name reaction validates a product need.

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

## Future-market boundary

Poland is the only current market. Germany, Austria, Switzerland, or other countries may be explored only after Poland supplies evidence for a useful and potentially repeatable solution. Keep future extension technically possible where that is naturally simple, but do not create multi-market architecture, content trees, deployments, or abstractions before a real second market exists.

## Guiding rule

Do not ask: "What application should we build?"

Ask: "What have we learned, and what is the smallest useful experiment that should happen next?"
