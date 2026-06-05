<!--
Sync Impact Report
- Version change: template-unversioned -> 1.0.0
- Modified principles:
	- Placeholder Principle 1 -> I. Code Quality Is a Product Feature
	- Placeholder Principle 2 -> II. Tests Define Done
	- Placeholder Principle 3 -> III. Consistent User Experience Across Adapters
	- Placeholder Principle 4 -> IV. Performance Budgets Are Mandatory
	- Placeholder Principle 5 -> V. Architecture-First Technical Decisions
- Added sections:
	- Implementation Constraints
	- Delivery Workflow and Quality Gates
- Removed sections:
	- None
- Templates requiring updates:
	- ✅ .specify/templates/plan-template.md
	- ✅ .specify/templates/spec-template.md
	- ✅ .specify/templates/tasks-template.md
	- ✅ .specify/templates/constitution-template.md (already compatible, no change required)
- Command templates requiring updates:
	- ✅ None found (.specify/templates/commands does not exist)
- Runtime guidance requiring updates:
	- ✅ README.md checked, no conflicting constitutional references
- Follow-up TODOs:
	- None
-->

# PixelSchubser Constitution

## Core Principles

### I. Code Quality Is a Product Feature

All production code MUST be readable, reviewable, and maintainable. Every change
MUST preserve layer boundaries defined in
requirements/architecture-principles.md, avoid hidden coupling, and include
clear names, focused units, and explicit error paths. New complexity MUST be
justified in the plan and reviewed against a simpler alternative.

Rationale: PixelSchubser is intended as a long-lived platform; low-quality code
directly slows feature delivery and increases defect risk.

### II. Tests Define Done

Testing is mandatory, not optional. Each implemented use case MUST include
appropriate automated tests at the right level: domain unit tests for business
rules, application/use-case tests for orchestration, and adapter-level tests for
contracts. Bug fixes MUST include a regression test that fails before the fix.

Rationale: The architecture demands UI-independent behavior. Only systematic
testing proves the same behavior across Avalonia, API, MCP, CLI, and headless
flows.

### III. Consistent User Experience Across Adapters

User-visible behavior MUST remain consistent across all adapters for equivalent
operations, including naming, validation semantics, error categories, and result
states. Any intentional adapter-specific variation MUST be documented in
specification acceptance criteria and reviewed before implementation.

Rationale: PixelSchubser is a multi-adapter system. Inconsistent behavior
between desktop, API, and MCP creates user confusion and automation drift.

### IV. Performance Budgets Are Mandatory

Performance-sensitive workflows MUST define measurable budgets in feature specs
before implementation. At minimum, specs MUST state expected latency or
throughput targets for interactive editing and bulk operations. Changes that
touch critical paths MUST include measurement evidence and MUST NOT introduce
unreviewed regressions.

Rationale: Responsive sprite editing and deterministic automation are core value
propositions and must be protected continuously.

### V. Architecture-First Technical Decisions

Technical decisions MUST follow the architecture principles and dependency rule.
Domain and Application layers MUST remain free of UI/framework-specific logic,
and adapters MUST remain thin. If a proposed implementation pressures these
boundaries, the team MUST record an explicit trade-off and obtain approval
before merge.

Rationale: Architecture integrity enables extensibility (web/API/MCP/headless)
without rewriting core logic.

## Implementation Constraints

- Features MUST expose behavior as Application use cases before adapter wiring.
- Public contracts (API endpoints, MCP tools, file formats) MUST be versioned
  and validated with contract-level tests where applicable.
- Error modeling MUST prefer explicit domain/application result types for
  expected business failures; exceptions remain for technical failures.
- UI work MUST not bypass Application ports for domain mutations.
- Performance and UX acceptance criteria MUST appear in each feature spec.

## Delivery Workflow and Quality Gates

- Plan Gate: Every plan MUST include constitution checks for code quality,
  required testing scope, UX consistency impact, and performance budgets.
- Spec Gate: Every spec MUST include independently testable scenarios and
  measurable outcomes for quality-relevant behavior.
- Task Gate: Tasks MUST include implementation, verification, and performance
  validation work where a critical path is affected.
- PR Gate: Reviews MUST explicitly confirm architecture compliance and test
  evidence. Missing evidence blocks merge.
- Release Gate: Regressions against accepted performance budgets or contract
  compatibility MUST be resolved or formally waived with documented rationale.

## Governance

This constitution overrides conflicting local conventions for planning,
implementation, and review in this repository.

Amendment process:

1. Propose a change with rationale and impacted principles/sections.
2. Update dependent templates and guidance in the same change set.
3. Record version bump rationale according to semantic rules below.
4. Obtain maintainer approval before the amendment is considered active.

Versioning policy:

- MAJOR: Backward-incompatible governance change or removal/redefinition of a
  core principle.
- MINOR: New principle/section added or substantial expansion of obligations.
- PATCH: Clarifications, wording improvements, and non-semantic refinements.

Compliance review expectations:

- Every PR review MUST include a constitution compliance check.
- Every implementation plan MUST pass constitution gates before execution.
- Non-compliance requires either remediation before merge or an explicit,
  time-bounded waiver approved by maintainers.

**Version**: 1.0.0 | **Ratified**: 2026-06-05 | **Last Amended**: 2026-06-05
