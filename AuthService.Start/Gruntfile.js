/// <binding BeforeBuild='all' ProjectOpened='watch' />
/**
 * Запускает задачу наблюдения при открытии проекта, все задачи перед 
 * билдом (выбирается в меню Task Runner)
 * @param grunt
 */
module.exports = function (grunt) {
    grunt.initConfig({
        
        /** очистка файлов какие папки/файлы очищать */
        clean: ["wwwroot/css/*", "wwwroot/js/app.min.js", "ScriptsAndCss/Combined/*"], 
        concat: {
            //объединение JS
            js: {
                //сюда можно писать файлы для объединения через запятую
                src: [
                    "ScriptsAndCss/JsScripts/**/*.js"
                ],
                //расположение объединенного файла
                dest: "ScriptsAndCss/Combined/combined.js" 
            },
            //объединение CSS
            css: {
                //сюда можно писать файлы для объединения через запятую
                src: ["ScriptsAndCss/CssFiles/*.css"],
                //расположение объединенного файла
                dest: "ScriptsAndCss/Combined/combined.css" 
            }
        },
        //сжатие JS
        uglify: { 
            js: {
                //какой файл сжимать
                src: ["ScriptsAndCss/Combined/combined.js"],
                //сжатый выходной файл
                dest: "wwwroot/js/app.min.js" 
            }
        },
        //сжатие CSS
        cssmin: { 
            css: {
                //какой файл сжимать
                src: ["ScriptsAndCss/Combined/combined.css"],
                //сжатый выходной файл
                dest: "wwwroot/css/app.min.css" 
            }
        },
        //наблюдение за изменениями
        watch: {
            //за изменением каких файлов наблюдаем
            files: ["ScriptsAndCss/JsScripts/**/*.js", "ScriptsAndCss/CssFiles/*.css"],
            //какую задачу запускаем
            tasks: ["all"] 
        }
    });

    //для очистки файлов
    grunt.loadNpmTasks("grunt-contrib-clean");
    //для объединения JS и CSS
    grunt.loadNpmTasks("grunt-contrib-concat");
    //для сжатия JS
    grunt.loadNpmTasks("grunt-contrib-uglify");
    //для сжатия CSS
    grunt.loadNpmTasks("grunt-contrib-cssmin");
    //общая задача
    grunt.registerTask("all", ["clean", "concat", "uglify", "cssmin"]);
    //для наблюдения за изменениями в файлах
    grunt.loadNpmTasks("grunt-contrib-watch"); 
};