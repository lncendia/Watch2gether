// import {useState} from 'react';
// import styles from "./Rating.module.css"
//
// const Rating = ({disabled = false, score, scoreChanged}: {
//     disabled?: boolean,
//     score?: number,
//     scoreChanged: (score: number) => void
// }) => {
//
//     const [score, changeScore] = useState(0)
//
//     return (
//         <div className={styles.rating_area}>
//             <input type="radio" id="star-10" value="10"/>
//             <label htmlFor="star-10" title="Оценка «10»"/>
//             <input type="radio" id="star-9" value="9"/>
//             <label htmlFor="star-9" title="Оценка «9»"/>
//             <input type="radio" id="star-8" value="8"/>
//             <label htmlFor="star-8" title="Оценка «8»"/>
//             <input type="radio" id="star-7" value="7"/>
//             <label htmlFor="star-7" title="Оценка «7»"/>
//             <input type="radio" id="star-6" value="6"/>
//             <label htmlFor="star-6" title="Оценка «5»"/>
//             <input type="radio" id="star-5" value="5"/>
//             <label htmlFor="star-5" title="Оценка «5»"/>
//             <input type="radio" id="star-4" value="4"/>
//             <label htmlFor="star-4" title="Оценка «4»"/>
//             <input type="radio" id="star-3" value="3"/>
//             <label htmlFor="star-3" title="Оценка «3»"/>
//             <input type="radio" id="star-2" value="2"/>
//             <label htmlFor="star-2" title="Оценка «2»"/>
//             <input type="radio" id="star-1" value="1"/>
//             <label htmlFor="star-1" title="Оценка «1»"/>
//         </form>
//     );
// };
//
// export default Rating;