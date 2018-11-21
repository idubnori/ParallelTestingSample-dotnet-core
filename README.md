ParallelTestingSample-dotnet
===
[![Build Status](https://dev.azure.com/idubnori/idubnori/_apis/build/status/ParallelTestingSample-dotnet-CI)](https://dev.azure.com/idubnori/idubnori/_build/latest?definitionId=4)

This dotnet parallel testing sample uses the `--list-tests` and `--filter` parameters of `dotnet test` to slice the tests. The tests are run using the NUnit.

## Overview of *azure-pipelines.yml*
 1. Set parallel count to job (i.e. 5)
```yml
jobs:
- job: 'ParallelTesting'
  pool:
    name: Hosted Ubuntu 1604
  strategy:
    parallel: 5
  displayName: Run tests in parallel
```
 2. Get test name list of all tests by using `--list-tests` parameter (100 in sample, Note that `grep Test_` to get test name only)
 3. `create_slicing_filter_condition.sh` creates filter condition of sliced tests, and set into `$(targetTestsFilter)` (20 in sample)
```yml
  - bash: |
      tests=($(dotnet test . --no-build --list-tests | grep Test_))
      . 'create_slicing_filter_condition.sh' $tests
    displayName: 'Create slicing filter condition'
```

 4. Use slicing filter condition in `dotnet test` command
 ```yml
  - task: DotNetCoreCLI@2
    displayName: Test
    inputs:
      command: test
      projects: '**/*Tests/*Tests.csproj'
      arguments: '--no-build --filter "$(targetTestsFilter)"'
```