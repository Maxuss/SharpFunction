function registerLanguage() {
    const monaco = window.monaco;

    monaco.languages.register({id: 'sflang'})

    monaco.languages.setMonarchTokensProvider('sflang', {
        tokenizer: {
            root: [
                [/"(?:[^\\]|\\.)*?"/, "string"],
                [/'(?:[^\\]|\\.)*?'/, "string"],
                [/(\/\*|\*\/)/, "comment"],
                [/\s*\/\/.*/, "comment"],
                [/\b[0-9.\-]+\b/, "number"],
                [/method\s*\(.*\)\s*/, "keyword"],
                [/(@[\w\-]*)/, "variable"],
                [/(@[a-zA-Z0-9]+)(?=\s*\(.*)/, "variable"],
                [/(\s*declare\s*(\(|)(['"].*['"])(\)|)\s*include\s*(\(|)(['"].*['"])(\)|))(\s+)([\w\d$\/._-]*)/, "operator"],
                [
                    /(null|true|false|undefined|NaN|error)/,
                    "property"
                ],
                [
                    /(if|else|loop|any|all|apply|slice|get|count|add|update|each|substr|replace|length|eval|out|command|vector|extract|let|set|const)/,
                    "keyword"
                ],
                [
                    /(list|map|string|number|json|sum|sub|mul|div|mod|pow|let|set|eq|mt|lt|mte|lte|not|extern)/,
                    "keyword2"
                ]
            ]
        }
    })

    monaco.languages.setLanguageConfiguration("sflang", {
        brackets: [
            ["{", "}"],
            ["(", ")"],
        ],
        comments: {
            lineComment: "//",
            blockComment: ['/*', '*/']
        },
        folding: {
            offSide: true,
        },
    });

// Taken and modified from mcscript from
// https://github.com/Stevertus/mcscript/blob/3f5fb17025de29b7cb76e20d3a660a5b866f6e9b/monaco%20monarch%20Highlighter.js#L99-L113
    monaco.editor.defineTheme("sflang-theme", {
        base: "vs-dark",
        inherit: true,
        rules: [
            {token: "number", foreground: "#97ecbc"},
            {token: "comment", foreground: "#7ca66e"},
            {token: "keyword", foreground: "#5872d9"},
            {token: "keyword2", foreground: "#9976d5"},
            {token: "string", foreground: "#e39a3c"},
            {token: "property", foreground: "#7d50c9"},
        ],
    });

    monaco.languages.registerCompletionItemProvider('sflang', {
        provideCompletionItems: () => {
            const suggestions = [
                {
                    label: 'mixin',
                    kind: monaco.languages.CompletionItemKind.Snippet,
                    insertText: 'const @${1:name} = extern(\'${2:assemblyName}\', \'${3:class}\')',
                    insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    documentation: 'Mixin declaration.\nImports an external mixin from provided assembly.'
                },
                {
                    label: 'constant',
                    kind: monaco.languages.CompletionItemKind.Snippet,
                    insertText: 'const @${1:name} = $0',
                    insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    documentation: 'Simple Constant Declaration'
                },
                {
                    label: 'variable',
                    kind: monaco.languages.CompletionItemKind.Snippet,
                    insertText: 'let @${1:name} = $0',
                    insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    documentation: 'Simple Variable Declaration'
                },
                {
                    label: 'map',
                    kind: monaco.languages.CompletionItemKind.Snippet,
                    insertText: [
                        'let @${1:name} = map(',
                        '    \'${2:key}\', \'${3:value}\'',
                        ')'
                    ].join('\n'),
                    insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    documentation: 'Simple Map Creation'
                }, {
                    label: 'method',
                    kind: monaco.languages.CompletionItemKind.Snippet,
                    insertText: [
                        'const @${1:name} = method(${2:params}) {',
                        '    $0',
                        '}'
                    ].join('\n'),
                    insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    documentation: 'Simple Method Creation'
                }, {
                    label: 'ifelse',
                    kind: monaco.languages.CompletionItemKind.Snippet,
                    insertText: [
                        'if (${1:condition}) {',
                        '    $0',
                        '} else {',
                        '    ',
                        '}'
                    ].join('\n'),
                    insertTextRules: monaco.languages.CompletionItemInsertTextRule.InsertAsSnippet,
                    documentation: 'If-Else Statement'
                }];
            return {suggestions: suggestions};
        }
    });
}