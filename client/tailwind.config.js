/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    extend: {
      gridTemplateColumns: {
        'custom': '0.5fr 1fr 0.5fr',
      }
    },
  },
  daisyui: {
    themes: ['light']
  },
  plugins: [
    require('daisyui')
  ],
}
