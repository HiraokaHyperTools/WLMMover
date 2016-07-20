; example1.nsi
;
; This script is perhaps one of the simplest NSIs you can make. All of the
; optional settings are left to their default settings. The installer simply 
; prompts the user asking them where to install, and drops a copy of example1.nsi
; there. 

;--------------------------------

!define APP "WLMMover"
!system 'DefineAsmVer.exe "bin\x86\DEBUG\${APP}.exe" "!define VER ""[SVER]"" " > Tmpver.nsh'
!include "Tmpver.nsh"
!searchreplace APV ${VER} "." "_"

; bin\x86\DEBUG

; The name of the installer
Name "${APP} ${VER}"

; The file to write
OutFile "Setup_${APP}_${APV}.exe"

; The default installation directory
InstallDir "$APPDATA\${APP}"

; Request application privileges for Windows Vista
RequestExecutionLevel user

;--------------------------------

; Pages

Page license
Page directory
Page instfiles

LicenseData "MS-PL.rtf"

;--------------------------------

; The stuff to install
Section "" ;No components page, name is not important

  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ClearErrors
  
  ; Put file there
  File /x "*.vshost.*" "bin\x86\DEBUG\*.*"
  
  Exec '"$INSTDIR\${APP}.exe"'
  
  IfErrors +2 0
    SetAutoClose true
  
SectionEnd ; end the section
