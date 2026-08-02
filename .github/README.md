# GitHub Actions overview

`workflows/parallel-testing-ci.yml` runs the same parallel test strategy as
`azure-pipelines.yml`.

## Triggers

The workflow runs for pull requests and pushes targeting `master`, on a weekly
schedule at 03:00 UTC on Monday, and can also be started manually from the
GitHub Actions page.

## Parallel test execution

The workflow creates five Ubuntu jobs with a matrix. Each job:

1. Checks out the repository and installs the SDK specified by `global.json`.
2. Restores dependencies and builds the project.
3. Lists NUnit tests and sources `create_slicing_filter_condition.sh`.
4. Sets the same `SYSTEM_TOTALJOBSINPHASE` and
   `SYSTEM_JOBPOSITIONINPHASE` values used by Azure Pipelines, so each job runs
   one fifth of the test suite.
5. Runs its selected tests with `dotnet test --no-build --filter`.

The matrix uses `fail-fast: false`, so all five slices finish and report their
results even when one slice fails.
