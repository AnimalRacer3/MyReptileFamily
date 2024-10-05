from _T_Web import create_app

app = create_app()

if __name__ == '__main__':
    app.run(debug=True)