# Terrarium Client

This client requires two COM Inproc Servers to be registered to the application to run correctly
Please run the following command from a commands line to register

```
regsvr32 $(SolutionDir)DXVBLib\dx7vb.dll
regsvr32 $(SolutionDir)DXVBLib\dx8vb.dll
```

where ```$(SolutionDir)``` is the path to the solution including a trailing backspace. 
These commands should be exected in an elevated command line.

I haven't included these as pre-build steps because the elevation requirement.
