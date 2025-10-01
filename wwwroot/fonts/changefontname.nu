#!/usr/bin/env nu

let pattern = (input "Enter the prefix to remove: ");

ls *.otf | get name | each {|f| let newname = ($f | str replace $'(($pattern))' '')
if $f != $newname {
  mv $f $newname
}}

