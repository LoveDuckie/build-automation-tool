<div align="center">

# build-automation-tool

</div>

**build-automation-tool** is a task automation tool that organizes and executes build operations through a task dependency graph, allowing for efficient and structured build workflows.

Originally developed as a technical assessment for a job interview, this project received positive feedback, which led to its open-sourcing. The tool is inspired by industry-standard build systems like **[Cake Build](https://cakebuild.net/)**, **[MSBuild](https://github.com/dotnet/msbuild)**, and **[BuildGraph](https://dev.epicgames.com/documentation/en-us/unreal-engine/buildgraph-for-unreal-engine)**. It leverages a [Directed Acyclic Graph (DAG)](https://en.wikipedia.org/wiki/Directed_acyclic_graph) to define dependencies and enforce a logical execution order of tasks, facilitating complex build pipelines.

## Demonstration

The **build-automation-tool** can operate in two execution modes: **parallel** and **sequential**. Both modes utilize the task dependency graph but differ in how tasks with shared dependencies are executed.

- **Parallel Execution:** Allows tasks that have shared dependencies but are independent of each other to run concurrently. This mode optimizes runtime by leveraging available resources, reducing the overall build time.
  
- **Sequential Execution:** Ensures all tasks are executed one after another, maintaining a strict chronological order based on task dependencies. This mode is ideal for processes where tasks are interdependent or when parallel execution is unnecessary.

Below are animated demonstrations that show how tasks progress in each mode.

<div align="center">

### Parallel Execution

![Parallel](taskrunner-parallel.gif)

In parallel execution mode, tasks are grouped and executed concurrently whenever possible. Tasks with dependencies that do not conflict are run simultaneously, reducing build times.

### Sequential Execution

![Sequential](taskrunner-sequential.gif)

Sequential execution mode processes each task in strict dependency order, from start to finish. This ensures a reliable and controlled build process where tasks execute one at a time.

</div>

With **build-automation-tool**, developers can efficiently manage complex build workflows, streamline repetitive build tasks, and improve productivity by automating dependency-based execution. Whether you’re building small applications or handling large projects with intricate dependencies, this tool provides a flexible and powerful solution for automating your build pipeline.

## Table of Contents

- [build-automation-tool](#build-automation-tool)
  - [Demonstration](#demonstration)
    - [Parallel Execution](#parallel-execution)
    - [Sequential Execution](#sequential-execution)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Dependencies](#dependencies)
  - [Usage](#usage)
  - [Instructions](#instructions)
    - [Tool Structure](#tool-structure)

## Features

- **Automated Build Execution:** Automatically runs a series of build operations defined by task dependencies, eliminating manual handling.
- **Flexible Task Management:** Supports both parallel and sequential execution, allowing tasks to run concurrently if dependencies permit.
- **Dependency-Driven Execution Flow:** Uses a DAG to manage the order of tasks, ensuring dependent tasks execute in the correct sequence.
- **Visual Feedback:** Animated examples of task execution illustrate how tasks progress through parallel and sequential workflows.
- **Extensible Script Library:** Contains modular scripts for various build tasks, making it adaptable to diverse project requirements.

## Dependencies

To run **build-automation-tool**, the following dependencies are required:

- **PowerShell**: All scripts are written in PowerShell. Ensure it is installed on your machine (PowerShell Core is recommended for cross-platform compatibility).
- **.NET Core SDK**: The tool relies on the .NET Core SDK for compiling and running tasks within the build pipeline. Install the appropriate version for your system to ensure compatibility.

## Usage

This tool includes several core PowerShell scripts to help manage different stages of the build and release process:

1. **Build.ps1** - Executes the main build process based on the task dependency graph defined for the project. This is the primary script for generating builds.
2. **BumpVersion.ps1** - Automates version incrementing for the project, ensuring version consistency across releases.
3. **Publish.ps1** - Packages the tool for distribution, creating a distributable package that can be easily shared or deployed.
4. **RunBuildAutomationTool.ps1** - The main script that initiates the build automation process. Use this script to start the tool with specified task parameters, which will dictate how the DAG is processed.

Each script is designed to be run independently but can also be chained to create a custom pipeline, depending on your project needs.

## Instructions

Detailed usage instructions are available in [INSTRUCTIONS.md](INSTRUCTIONS.md). This document includes examples, parameters, and advanced configurations to help users maximize the tool’s capabilities in various environments.

### Tool Structure

The *Tool* folder organizes the core source files for the **Build Automation Tool**. This folder includes the scripts and configurations necessary to execute build tasks as defined in the technical requirements. Each file is crafted to streamline the execution process, following the task dependencies outlined in the DAG.
