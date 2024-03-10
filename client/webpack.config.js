module.exports = {
    entry: './src/app.ts',
    module: {
        rules: [
            { test: /\.tsx?$/, use: 'ts-loader' },
        ],
    },
};