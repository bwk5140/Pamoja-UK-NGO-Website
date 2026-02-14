module.exports = {
    theme: {
        extend: {
            keyframes: {
                marquee: {
                    '0%': { transform: 'translateX(100%)' },
                    '100%': { transform: 'translateX(-100%)' },
                },
            },
            animation: {
                marquee: 'marquee 15s linear infinite',
            },
            colors: {
                primary: {
                    25: '#2E7D32',
                    50: '#f0fdfa',
                    100: '#ccfbf1',
                    200: '#99f6e4',
                    300: '#5eead4',
                    400: '#2dd4bf',
                    500: '#14b8a6',
                    600: '#0d9488',
                    700: '#0f766e',
                    800: '#115e59',
                    900: '#134e4a',
                },
                secondary: {
                    25: '#1976D2',
                    50: '#fff7ed',
                    100: '#ffedd5',
                    200: '#fed7aa',
                    300: '#fdba74',
                    400: '#fb923c',
                    500: '#f97316',
                    600: '#ea580c',
                    700: '#c2410c',
                    800: '#9a3412',
                    900: '#7c2d12',
                },
                accent: '#FF9800',
                fontFamily: {
                    'sans': ['Inter', 'system-ui', 'sans-serif'],
                    'serif': ['Source Serif Pro', 'Georgia', 'serif']
                },
                spacing: {
                    '128': '32rem',
                    '144': '36rem'
                }
            },
        },
    },
};