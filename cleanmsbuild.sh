#!/usr/bin/bash
find . -type d -iname "bin" -exec rm -rf {} \;
find . -type d -iname "obj" -exec rm -rf {} \;
