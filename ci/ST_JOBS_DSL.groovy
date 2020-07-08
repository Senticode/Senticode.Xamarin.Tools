def nuget = $/C:\_tools\nuget\5.0.2\nuget.exe/$
def msbuild = 'VS-2019-x32'

job('SXT_NEW_RELEASE') {
	parameters {
		stringParam('VERSION', null, 'Version of the new release')
	}
	label('node-winserver19')
	scm {
        git {
		    remote {			   
               url('https://github.com/Senticode/Senticode.Xamarin.Tools.git')
               credentials('github')
			}   
			branch('dev')	             
		    extensions {               
				wipeOutWorkspace()
				cloneOptions {					 
					noTags(true)
                }
				userIdentity {
                    name('buildrobot')
					email('buildrobot@senticode.com')
                }   
			}            
		}	  	
    }
	steps {
		powerShell(readFileFromWorkspace($/ci\batchs\set_new_assembly_version.ps1/$))
		environmentVariables {
            propertiesFile('env.properties')             
        }
		buildNameUpdater {
            fromFile(false)
            fromMacro(true)
            macroTemplate('#${ASSEMBLY_VERSION}')
            macroFirst(false)
            buildName('#${ASSEMBLY_VERSION}')            
        }
        changeAsmVer {
            versionPattern('${ASSEMBLY_VERSION}')
			assemblyFile($/sln\SharedAssemblyInfo.cs/$)
        }         
        batchFile($/${nuget} restore %WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
        msBuild {				
			msBuildInstallation(msbuild)
			buildFile($/%WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
			args('/p:Configuration=Debug')
		}		
		powerShell(readFileFromWorkspace($/ci\batchs\change_assemblyinfo.ps1/$))		
		msBuild {
			msBuildInstallation(msbuild)
			buildFile($/%WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
			args('/p:Configuration=Release')
			args('/p:SignAssembly=true')
			args($//p:AssemblyOriginatorKeyFile=C:\jenkins\sgKey.snk/$)
		}		
		powerShell(readFileFromWorkspace($/ci\batchs\copy_artifacts.ps1/$))
		powerShell(readFileFromWorkspace($/ci\batchs\git_push_release.ps1/$))			
	}	
	publishers {		
        s3Upload {
            profileName('Administrator')
            dontWaitForConcurrentBuildCompletion(false)
			dontSetBuildResultOnFailure(true)
            consoleLogLevel('INFO')
            pluginFailureResultConstraint('UNSTABLE')
            entries {
                entry {
                    bucket('senticode.xamarin.tools/builds/release/release/${ASSEMBLY_VERSION}')
                    sourceFile('senticode.xamarin.tools-${ASSEMBLY_VERSION}-signed-dev.zip, senticode.xamarin.tools-${ASSEMBLY_VERSION}-signed.zip')
                    storageClass('STANDARD')
                    selectedRegion('us-east-2')
                    noUploadOnFailure(true)
                    excludedFile('')
                    flatten(false)
                    gzipFiles(false)
                    uploadFromSlave(false)
                    managedArtifacts(false)
                    useServerSideEncryption(false)
                    keepForever(false)
                    showDirectlyInBrowser(false)
                }
                entry {
                    bucket('senticode.xamarin.tools/builds/release/debug/${ASSEMBLY_VERSION}')
					sourceFile('senticode.xamarin.tools-${ASSEMBLY_VERSION}-unsigned.zip')
					storageClass('STANDARD')
					selectedRegion('us-east-2')
					noUploadOnFailure(true)
                    excludedFile('')
                    flatten(false)
                    gzipFiles(false)
                    uploadFromSlave(false)
                    managedArtifacts(false)
                    useServerSideEncryption(false)
                    keepForever(false)
                    showDirectlyInBrowser(false)
                }
            }
        }
    }		
}

job('SXT_NIGHTLY_BUILD') {
    label('node-winserver19')     
    scm {
        git {
		    remote {			   
               url('https://github.com/Senticode/Senticode.Xamarin.Tools.git')
               credentials('github')
			}   
			branch('dev')             
		    extensions {               
				wipeOutWorkspace()
				cloneOptions {
					shallow(true)
			        depth(0) 
					noTags(true)
				}                
                userIdentity {
                    name('buildrobot')
					email('buildrobot@senticode.com')
                }                
                userExclusion {
                    excludedUsers('buildrobot')
                }                
			}            
		}	  	
    }
    triggers {
        scm('TZ=Europe/Minsk\n0 3 * * *')        
    }
    steps {
        powerShell(readFileFromWorkspace($/ci\batchs\get_assembly_version.ps1/$))					
        environmentVariables {
            propertiesFile('env.properties')             
        }        
        buildNameUpdater {
            fromFile(false)
            fromMacro(true)
            macroTemplate('#${ASSEMBLY_VERSION}')
            macroFirst(false)
            buildName('#${ASSEMBLY_VERSION}')            
        }
        changeAsmVer {
            versionPattern('${ASSEMBLY_VERSION}')
			assemblyFile($/sln\SharedAssemblyInfo.cs/$)
        }
		changeAsmVer {
            versionPattern('${BASE_VERSION}')
			assemblyFile($/sln\BaseAssemblyInfo.cs/$)
        }  
        batchFile($/${nuget} restore %WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
		msBuildSQRunnerBegin {
			projectKey('SXT')
			projectName('Senticode.Xamarin.Tools')
		}
        msBuild {
			msBuildInstallation(msbuild)
			buildFile($/%WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
			args('/p:Configuration=Debug')
		} 
		msBuildSQRunnerEnd()		
		powerShell(readFileFromWorkspace($/ci\batchs\change_assemblyinfo.ps1/$))		
		msBuild {
			msBuildInstallation(msbuild)
			buildFile($/%WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
			args('/p:Configuration=Release')
			args('/p:SignAssembly=true')
			args($//p:AssemblyOriginatorKeyFile=C:\jenkins\sgKey.snk/$)
		}		
		powerShell(readFileFromWorkspace($/ci\batchs\copy_artifacts.ps1/$))
		powerShell(readFileFromWorkspace($/ci\batchs\git_push.ps1/$))		
    }	
    publishers {		
        s3Upload {
            profileName('Administrator')
            dontWaitForConcurrentBuildCompletion(false)
			dontSetBuildResultOnFailure(true)
            consoleLogLevel('INFO')
            pluginFailureResultConstraint('UNSTABLE')
            entries {
                entry {
                    bucket('senticode.xamarin.tools/builds/nightly/release/${ASSEMBLY_VERSION}')
                    sourceFile('senticode.xamarin.tools-${ASSEMBLY_VERSION}-signed-dev.zip, senticode.xamarin.tools-${ASSEMBLY_VERSION}-signed.zip')
                    storageClass('STANDARD')
                    selectedRegion('us-east-2')
                    noUploadOnFailure(true)
                    excludedFile('')
                    flatten(false)
                    gzipFiles(false)
                    uploadFromSlave(false)
                    managedArtifacts(false)
                    useServerSideEncryption(false)
                    keepForever(false)
                    showDirectlyInBrowser(false)
                }
                entry {
                    bucket('senticode.xamarin.tools/builds/nightly/debug/${ASSEMBLY_VERSION}')
					sourceFile('senticode.xamarin.tools-${ASSEMBLY_VERSION}-unsigned.zip')
					storageClass('STANDARD')
					selectedRegion('us-east-2')
					noUploadOnFailure(true)
                    excludedFile('')
                    flatten(false)
                    gzipFiles(false)
                    uploadFromSlave(false)
                    managedArtifacts(false)
                    useServerSideEncryption(false)
                    keepForever(false)
                    showDirectlyInBrowser(false)
                }
            }
        }
    }	
}

job('SXT_PUBLISH_NUGETS') {	
	label('node-winserver19')
	scm {
        git {
		    remote {			   
               url('https://github.com/Senticode/Senticode.Xamarin.Tools.git')
               credentials('github')
			}   
			branch('dev')	             
		    extensions {               
				wipeOutWorkspace()
				cloneOptions {					 
					noTags(true)
                }
				userIdentity {
                    name('buildrobot')
					email('buildrobot@senticode.com')
                }   
			}            
		}	  	
    }
	steps {
		batchFile($/${nuget} restore sln\Senticode.Xamarin.Tools.sln/$)
		powerShell(readFileFromWorkspace($/ci\batchs\get_nugets_version.ps1/$))
		powerShell(readFileFromWorkspace($/ci\batchs\change_assemblyinfo.ps1/$))
		environmentVariables {
            propertiesFile('env.properties')             
        }  
      	msBuild {
			msBuildInstallation(msbuild)
			buildFile($/%WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
			args('/p:Configuration=Release')
			args('/p:SignAssembly=true')
			args($//p:AssemblyOriginatorKeyFile=C:\jenkins\sgKey.snk/$)
		}		
		powerShell(readFileFromWorkspace($/ci\batchs\create_nugets.ps1/$))			
	}	
	publishers {		
        nugetPublisher {
			doNotFailIfNoPackagesArePublished(false)
			name('senticode.tools')
			nugetPublicationName('nuget.org')
			packagesExclusionPattern('')
			packagesPattern('*.nupkg')
			publishPath('')
		}
    }		
}

job('SXT_PUBLISH_BASE_NUGET') {	
	label('node-winserver19')
	scm {
        git {
		    remote {			   
               url('https://github.com/Senticode/Senticode.Xamarin.Tools.git')
               credentials('github')
			}   
			branch('dev')	             
		    extensions {               
				wipeOutWorkspace()
				cloneOptions {					 
					noTags(true)
                }
				userIdentity {
                    name('buildrobot')
					email('buildrobot@senticode.com')
                }   
			}            
		}	  	
    }
	steps {
		batchFile($/${nuget} restore sln\Senticode.Xamarin.Tools.sln/$)
		powerShell(readFileFromWorkspace($/ci\batchs\get_base_nuget_version.ps1/$))
		powerShell(readFileFromWorkspace($/ci\batchs\change_assemblyinfo.ps1/$))
		environmentVariables {
            propertiesFile('env.properties')             
        }  
      	msBuild {
			msBuildInstallation(msbuild)
			buildFile($/%WORKSPACE%\sln\Senticode.Xamarin.Tools.sln/$)
			args('/p:Configuration=Release')
			args('/p:SignAssembly=true')
			args($//p:AssemblyOriginatorKeyFile=C:\jenkins\sgKey.snk/$)
		}		
		powerShell(readFileFromWorkspace($/ci\batchs\create_base_nuget.ps1/$))			
	}	
	publishers {		
        nugetPublisher {
			doNotFailIfNoPackagesArePublished(false)
			name('senticode.tools')
			nugetPublicationName('nuget.org')
			packagesExclusionPattern('')
			packagesPattern('*.nupkg')
			publishPath('')
		}
    }		
}