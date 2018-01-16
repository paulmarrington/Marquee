# Marrington App Collection

Building requires two directory trees - one for source and one for development. Development need not be backed up. To simplify documentation, I will assume these are ***~/dev*** and ***~/src***.

## Prerequisites

1. A GitHub Client
2. A local clone of ***~/src/App-Common ***(git@github.com:paulmarrington/App-Common.git)
3. A Bash shell (use [http://win-bash.sourceforge.net/] or similar for Windows)
4. Install any frameworks needed (such as Unity)

## Creating a Development Project

1. Use GitHub to clone the package you want to work on
2. Run ***~/src/App-Common/Support/App-Install-Menu.sh***
3. Set development directory (d)
4. Fetch packages (f)
5. Enter App name (a)
6. Build a development copy (u)

The ***~/dev/package-name*** will use soft links to reference directories from ***~/src/package-name***.