#!/bin/bash

# TODO: Run all tests before deciding to return zero / non-zero exit code. 

testProjects=`ls -d1 ./test/*.Tests`
for testProject in $testProjects; do
	echo "Running unit tests for project \"$testProject\"."
	dnx -p "$testProject" test -verbose
	EXITCODE=$?

	if [ $EXITCODE != 0 ]; then
		echo "dnx test returned exit code ${EXITCODE}."

		exit 1
	fi
done

functionalTestProjects=`ls -d1 ./test/*.FunctionalTests`
for testProject in $testProjects; do
	echo "Running functional tests for project \"$testProject\"."
	dnx --appbase "./src/VersionManagement" -p "$testProject" test -verbose
	EXITCODE=$?
	
	if [ $EXITCODE != 0 ]; then
		echo "dnx test returned exit code ${EXITCODE}."
		 
		exit 1
	fi
done

echo "Done."
