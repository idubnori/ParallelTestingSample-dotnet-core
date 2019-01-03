ParallelTestingSample-dotnet-core
===
[![GitHub license](https://img.shields.io/github/license/idubnori/ParallelTestingSample-dotnet-core.svg)](https://github.com/idubnori/ParallelTestingSample-dotnet-core/blob/master/LICENSE)
[![Build Status](https://dev.azure.com/idubnori/idubnori/_apis/build/status/ParallelTestingSample-dotnet-CI)](https://dev.azure.com/idubnori/idubnori/_build/latest?definitionId=4)

This .NET Core parallel testing sample uses `--list-tests` and `--filter` parameters of `dotnet test` to slice the tests. The tests are run using the NUnit.

This sample has the 100 tests, and slices them to 20 tests in 5 jobs. You can see the pipeline behavior result by clicking the build status badge above.

## Overview of *azure-pipelines.yml*
### Setting up parallel count
```yml
jobs:
- job: 'ParallelTesting'
  pool:
    name: Hosted Ubuntu 1604
  strategy:
    parallel: 5
  displayName: Run tests in parallel
```

### Make slicing condition
 - Get test name list of all tests by using `--list-tests` parameter and `grep Test_`
 - `create_slicing_filter_condition.sh` makes filter condition to slice the tests, and set into `$(targetTestsFilter)`
```yml
  - bash: |
      tests=($(dotnet test . --no-build --list-tests | grep Test_))
      . 'create_slicing_filter_condition.sh' $tests
    displayName: 'Create slicing filter condition'
```

### Run tests using slicing condition
 ```yml
  - task: DotNetCoreCLI@2
    displayName: Test
    inputs:
      command: test
      projects: '**/*Tests/*Tests.csproj'
      arguments: '--no-build --filter "$(targetTestsFilter)"'
```

## References
 - [Speed up testing by running tests in parallel - Azure Pipelines & TFS | Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/test/parallel-testing-any-test-runner?view=vsts)