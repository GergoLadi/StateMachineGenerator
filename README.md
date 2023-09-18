# State Machine Generator
A tool to generate random state machines having configurable properties. The algorithm is loosely based on the Erdős-Rényi model [1] and was used to supply inputs for our tests in [2]. The utility can generate state machines with a set number of states and a desired density of self transitions and regular transitions. The algorithm ensures that all the states are reachable, that there is at least one valid transition from every state, and that the resulting state machine is deterministic (i.e. there are no states where the same input can lead to two different states).

## Usage
Simply run the utility to see the required and optional parameters along with valid ranges (where applicable).

```
Usage: StateMachineGenerator <numStates> <pctSelfLoop> <pctRandomEdges> <pctReuseInputChar> <pctReuseOutputChar> [outputFileName]
  
Parameters:
    numStates (integer, 2+, required): The number of states in the state machine.
    pctSelfLoop (integer, 0-100, required): The probability (in %) that a state will have a self loop.
    pctRandomEdges (integer, 0-100, required): The probability (in %) that two states will be connected.
    pctReuseInputChar (integer, 0-100, required): The probability (in %) that a previously used input character will be reused in a transition.
    pctReuseOutputChar (integer, 0-100, required): The probability (in %) that a previously used output character will be reused in a transition.
    outputFileName (string, optional): If provided, the output will be written to this file instead of being printed to the console.
```

## Example
#### Command line
```
.\StateMachineGenerator.exe 6 20 30 20 50
```
#### Output
```
S0
S1
S2
S3
S4
S5

S0      S1      A       a
S1      S2      B       b
S0      S3      C       c
S3      S4      D       d
S3      S5      A       e
S1      S1      C       f
S2      S0      C       f
S4      S0      E       g
S5      S0      F       g
```

#### Format Specifications
 - The output is made up of two blocks separated by an empty line
 - The first block consists of *n* lines, with each line containing the name of a state
 - The second block consists of *m* lines, with each line describing a transition, using TAB (\t) characters as separators
   - Source state
   - Destination state
   - Input (that triggers this transition)
   - Output (that is emitted during this transition)

## Building from Source
### With Visual Studio
  - Open *src/StateMachineGenerator.sln*
  - Select *Release* as the active build configuration
  - Build the solution by selecting *Build Solution* in the *Build* menu
  - The compiled executable will be placed in *StateMachineGenerator/bin/Release/<dotnet-version>*

### Using Command Line Tools
```sh
$ cd src
$ dotnet build -c Release
```

## References
- [1] [Pál Erdős, Alfréd Rényi: On Random Graphs I. (1959)](https://doi.org/10.5486%2FPMD.1959.6.3-4.12)
- [2] Gergő Ládi, Tamás Holczer: On the Performance Evaluation of Protocol State Machine Reverse Engineering Methods (2023) (publication in progress)
