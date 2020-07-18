# ZoomCOMPersist
Create COM persistence using Zoom and zero-width space directories.

Will create a new `Zoom` directory in the user's `%APPDATA%` directory and create a new registry key for the `{62BE5D10-60EB-11d0-BD3B-00A0C911CE86}`. Whenever Zoom.exe is launched, the registry key will be read and your DLL will be loaded and executed by Zoom.

## Usage
`ZoomCOMPersist.exe <base64 encoded DLL file>`

If you do not provide base64 encoded bytes of a DLL file, ZoomCOMPersist will create a DLL that startes a new cmd.exe process.

Example:
`ZoomCOMPersist.exe TVqQA...(snip)...AAAAAA=`
