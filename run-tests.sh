#!/bin/bash

testProjects=`ls -d1 ./test/*.Tests`
for testProject in $testProjects; do
	echo "Running unit tests for project \"$testProject\"."
	dnx -p "$testProject" test -verbose
done

functionalTestProjects=`ls -d1 ./test/*.FunctionalTests`
for testProject in $testProjects; do
	echo "Running functional tests for project \"$testProject\"."
	dnx --appbase "./src/VersionManagement" -p "$testProject" test -verbose
done

echo "Done."
