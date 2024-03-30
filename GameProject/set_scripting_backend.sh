#!/bin/bash

function GetValue {
    scriptingBackend=$1
    if [ "${scriptingBackend,,}" = "mono" ]; then
        echo 0
    elif [ "${scriptingBackend,,}" = "il2cpp" ]; then
        echo 1
    else
        echo -1
    fi
}

function Set-ScriptingBackend {
    projectSettingsPath=$1
    platform=$2
    scriptingBackend=$3

    if [[ $platform == "Standalone"* ]]; then
        platform="Standalone"
    fi

    IFS=$'\n'
    mapfile -t contentsList < "$projectSettingsPath"

    for ((i=0; i<${#contentsList[@]}; i++)); do
        if [[ ${contentsList[i]} =~ "scriptingBackend:" ]]; then
            unset 'contentsList[i]'
            ((i++))
            while [[ $i -lt ${#contentsList[@]} && ${contentsList[i]} =~ "    " ]]; do
                unset 'contentsList[i]'
                ((i++))
            done
            break
        fi
    done

    contentsList+=("  scriptingBackend:" "    $platform: $(GetValue $scriptingBackend)")
    printf "%s\n" "${contentsList[@]}" > "$projectSettingsPath"
}

Set-ScriptingBackend "$1" "$2" "$3"