const path = require('path');

module.exports = [
  {
    name: 'dev',
    mode: 'development',
    entry: '../WebWithFileApiExample/wwwroot/app/main.js',
    output: {
      filename: 'main.js',
      path: path.resolve(__dirname, 'dist/dev/app'),
    },
  },
  {
    name: 'prod',
    mode: 'production',
    entry: '../WebWithFileApiExample/wwwroot/app/main.js',
    output: {
      filename: 'main.js',
      path: path.resolve(__dirname, '../WebWithFileApiExample/bin/Release/net7.0/win-x64/publish/wwwroot/app'),
    },
  }
];