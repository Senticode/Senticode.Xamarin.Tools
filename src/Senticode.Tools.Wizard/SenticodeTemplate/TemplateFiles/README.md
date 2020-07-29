# README #

This README would normally document whatever steps are necessary to get your application up and running.

### What is this repository for? ###

* Quick summary
* Version

### How do I get set up? ###

* Summary of set up
* Configuration
* Dependencies
* Database configuration
* How to run tests
* Deployment instructions

### Contribution guidelines ###

* Folder convention
	* ./out/ – output directory to build artifacts;	
	* ./ci/ – contains maintenance scripts for continues integration/delivery process;
	* ./sln/ – contains .sln visual studio files and assembly info files;
	* ./src/ – main directory which, contains folders with projects;	
	* ./docs/ – contains some documents to development;
	* ./artifacts/ – contains external artifacts (assemblies etc.).

* Namespace Convention
	* [SolutionName].Common – projects that can be directly referenced by any other projects.
	* [SolutionName].Web – projects related only to the Web part of the solution.
	* [SolutionName].Mobile – projects related only to the Xamarin Forms App to Mobile part of the solution.
	* [SolutionName].Tests – projects with tests infrastructure.
	* ../Modules/ - contains some projects with service implementations from common interfaces. These projects can only be referenced by a special ModuleAggregator.
	* Tools – specified infrastructure which can be directly referenced by any other projects, but not depends from other logic of application.

* Versioning convention.

    Version information for an assembly consists of the following four values: [Major Version], [Minor Version], [Patch Version], [Build Number]. You should change
	* Major Version when you make incompatible API changes
	* Minor Version when you add functionality in a backwards-compatible manner
	* Patch Version when you make backwards-compatible _bug fixes
	* Build Number is incremented automatically by Continuous Integration server