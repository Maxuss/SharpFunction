monaco.languages.register({ id: 'sflang' })

monaco.languages.setMonarchTokensProvider('sflang', {
    tokenizer: {
        root: [
            [/"(?:[^\\]|\\.)*?"/, "string"],
            [/'(?:[^\\]|\\.)*?'/, "string"],
            [/\/\*/, "comment"],
            [/\*\\/, "comment"],
            [/\b[0-9.\-]+\b/, "number"],
            [/(method)(\s+)([\w\d$\/._-]*)/, "keyword"],
            [/(@[\w\-]*)/, "variable"],
            [/(@[a-zA-Z0-9]+)(?=\s*\(.*)/, "variable"],
            [/\s*\/\/.*/, "comment"],
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
                /(list|map|string|number|json|sum|sub|mul|div|mod|pow|let|set|eq|mt|lt|mte|lte|not)/,
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
        { token: "number", foreground: "#02ABFD" },
        { token: "comment", foreground: "#4f4f4f" },
        { token: "keyword", foreground: "#ffa640" },
        { token: "keyword2", foreground: "#209CDF" },
        { token: "string", foreground: "#00d262" },
        { token: "property", foreground: "#8FB900" },
    ],
});

monaco.languages.registerCompletionItemProvider('sflang', {
    provideCompletionItems: () => {
        const suggestions = [
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
        return { suggestions: suggestions };
    }
});