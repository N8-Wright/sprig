module.exports = {
    entry: './src/app.ts',
    module: {
        rules: [
            { test: /\.ts$/, use: 'ts-loader' },
        ],
    },
    resolve: {
        extensions: ['.ts', '...'],
    },
};