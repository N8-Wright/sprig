const path = require('path');

module.exports = {
    mode: 'production',
    entry: './src/app.ts',
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
            },
        ],
    },
    resolve: {
        extensions: ['.ts'],
    },
    output: {
        filename: 'app.js',
        path: path.resolve(__dirname, 'dist'),
    },
};