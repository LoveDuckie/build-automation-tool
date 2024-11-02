<div align="center">

# build-automation-tool

</div>

A tool for automating a sequence of build operations defined as a task dependency graph.

This project was originally developed as a test sample for a job interview process. The project was well-received, and I decided to open-source the tool to share the effort invested in building it.

This project borrows ideas from **[Cake Build](https://cakebuild.net/)**, **[MSBuild](https://github.com/dotnet/msbuild)**, and **[BuildGraph](https://dev.epicgames.com/documentation/en-us/unreal-engine/buildgraph-for-unreal-engine)**, in that it uses a [Directed-Acyclic Graph](https://en.wikipedia.org/wiki/Directed_acyclic_graph) for describing the chronological order for the invocation of defined tasks.