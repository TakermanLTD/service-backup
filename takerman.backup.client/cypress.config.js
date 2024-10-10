const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    baseUrl: 'https://localhost:5178',
    setupNodeEvents(on, config) {
    }
  },
  component: {
    devServer: {
      framework: 'vue',
      bundler: 'vite',
    },
  },
  projectId: "y7snqm"
});
