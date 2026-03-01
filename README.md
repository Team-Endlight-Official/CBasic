# CBasic - Transpiled, Fast and Easy to learn
CBasic is the new form of BASIC-like languages with more options and scalability. CBasic is constructed from 4 main layers - those are: C++ compiler, CBasic runtime in C++, Function-Parameter-Map (see below) and the CLI which calls the shots!

## Setup
To use CBasic in it's purest form you need to have a valid C++ 17 compiler.
Download the CBasic CLI which has a built-in text editor, and the bundled CBasic Runtime in it.

# Proof of concept
The way CBasic is a transpiled compiled language and fast as C++ is by leveraging existing language design architectures but with a little more to it. The runtime is written in C++ for the promised speed, the CLI (+ Text Editor) is written in C# for development efficiency and scalability. The CLI not only has the editor built in, but the transpiler and error checker as well. Now you might ask how is C# gonna read C++ code from the runtime .dll. Well i have an answer and it is Function-Parameter-Map or FPM for short. FPM is essentially a .json file that maps the function name, it's parameter types and the return value.
For example:
```json
{
  "cb_print": {
    "params": ["string"],
    "return": "void"
  },
  "cb_replace": {
    "params": ["string", "string"],
    "return": "void"
  },
  etc..
}
```
Which in fact allows for extension of user made CBasic extensions/libraries written in C++ using this exact workflow.
